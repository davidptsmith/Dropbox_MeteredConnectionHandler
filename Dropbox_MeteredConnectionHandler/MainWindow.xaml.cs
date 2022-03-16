using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.NetworkInformation;
using Windows.Networking.Connectivity;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Dropbox_MeteredConnectionHandler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // check for metered conneciton
            CheckIsMetered();

            //check to see if dropbox is running

            //prompt user to close dropbox

            //if yes, close dropbox

            if (IsMetered)
            {
            NetworkInformation.NetworkStatusChanged += (s) => CheckIsMetered();

            //show user window 
            InitializeComponent();
            }
            
         
        }


        private bool isInternetAvailable;

        public bool IsInternetAvailable
        {
            get { return isInternetAvailable; }
            set { isInternetAvailable = value; }
        }

        public bool IsMetered { get; private set; }

        private void CheckIsMetered()
        {
            

            var profile = Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile();
            IsInternetAvailable = profile != null && profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;

            if (IsInternetAvailable)
                IsMetered = profile.GetConnectionCost().NetworkCostType != Windows.Networking.Connectivity.NetworkCostType.Unrestricted;
            

            if (IsMetered)
            {
                closeDropbox();

            }
        }


        private void closeDropbox()
        {
            try
            {
                foreach (Process Proc in Process.GetProcesses())
                    if (Proc.ProcessName.ToUpper().Contains("DROPBOX"))  //Process Excel?
                    {
                        //Proc.Kill();
                        Proc.Kill(); 
                        Proc.WaitForExit(); 
                    }

            }
            catch (Exception er)
            {
                var msg = er.Message;
                MessageBox.Show(msg, "Error Closing Dropbox"); 

            }
        }
      

        private void btn_click_closeDropbox(object sender, RoutedEventArgs e)
        {
            closeDropbox();
        }
    }
}
