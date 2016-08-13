using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IP_Info
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Default Gateway..................: {0}\n", GetDefaultGateway());
                Console.WriteLine("Local IP(V4) Address.............: {0}\n", GetLocalIPv4(NetworkInterfaceType.Ethernet).FirstOrDefault().ToString());
                Console.WriteLine("External (WAN) IP Address........: {0}\n", GetExternalIP());

                System.Threading.Thread.Sleep(10000);

            }
        }

        public static string GetExternalIP()
        {
            string externalip = "Cannot Connect";

            try { externalip = new WebClient().DownloadString("http://icanhazip.com"); }
            catch
            {
                try
                {
                    externalip = new WebClient().DownloadString("http://bot.whatismyipaddress.com");
                }
                catch
                {
                    try { externalip = new WebClient().DownloadString("http://icanhazip.com"); }
                    catch { }
                }

            }
            return externalip;
        }
        public static string GetDefaultGateway()
        {
            try
            {
                var card = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
                if (card == null) return null;
                var address = card.GetIPProperties().GatewayAddresses.FirstOrDefault();
                return address.Address.ToString();
            }
            catch
            {
                return "Cannot get network info.";
            }
        }
        

        public static string[] GetLocalIPv4(NetworkInterfaceType _type)
        {
            List<string> ipAddrList = new List<string>();
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddrList.Add(ip.Address.ToString());
                        }
                    }
                }
            }
            return ipAddrList.ToArray();
        }
    }
}
