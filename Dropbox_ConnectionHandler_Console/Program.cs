using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using Windows.Networking.Connectivity;



namespace Dropbox_ConnectionHandler_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Checking Connection Type");
            CheckIsMetered();
            NetworkInformation.NetworkStatusChanged += (s) => CheckIsMetered();
            

            while (true)
            {
                Console.WriteLine("checking...");
                if(IsMetered)
                    closeDropbox();

                Thread.Sleep(10000); 
            }
        }


        private static bool isInternetAvailable;

        public static bool IsInternetAvailable
        {
            get { return isInternetAvailable; }
            set { isInternetAvailable = value; }
        }

        public static bool IsMetered { get; private set; }

        private static void CheckIsMetered()
        {
            try
            {

                Console.WriteLine("Checking Connection");
            var profile = Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile();
            IsInternetAvailable = profile != null && profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;

            if (IsInternetAvailable)
            {
                Console.WriteLine("Connection is Available");
                IsMetered = profile.GetConnectionCost().NetworkCostType != Windows.Networking.Connectivity.NetworkCostType.Unrestricted;
            }


            if (IsMetered)
            {
                Console.WriteLine("Connection Type Is Metered");
                closeDropbox();

            }
            }
            catch(Exception er)
            {
                var msg = er.Message;
                Console.WriteLine("Error Closing Dropbox:");
                Console.WriteLine(msg);
            }
        }


        private static void closeDropbox()
        {
            try
            {
                Console.WriteLine("Trying to find and close Dropbox");
                foreach (Process Proc in Process.GetProcesses())
                    if (Proc.ProcessName.ToUpper().Equals("DROPBOX"))  //Process Excel?
                    {
                        Console.WriteLine("Found Dropbox");
                        Console.WriteLine("Killing Dropbox");
                        //Proc.Kill();
                        Proc.Kill();
                        Console.WriteLine("Waiting for Dropbox to exit");
                        Proc.WaitForExit();
                        Console.WriteLine("Dropbox Closed");
                    }

            }
            catch (Exception er)
            {
                var msg = er.Message;
                Console.WriteLine( "Error Closing Dropbox:");
                Console.WriteLine(msg);

            }
        }


    }
}
