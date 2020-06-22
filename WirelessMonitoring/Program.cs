using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Management;
using System.Threading;

namespace WirelessMonitoring
{
    class Program
    {
        const int server_port = 8005;
        const string server_ip = "192.168.1.68";

        static string cpu_name, cpu_cores, cpu_threads,cpu_percent, pc_name, ram_total, ram_available;
        static int screen_height, screen_width;
        static double ram_percent;
        public delegate void UpdateTBCallback(int key,string text);
        static public void Start()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(server_ip), server_port);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(0);
                // Server is started
                //MainWindow.ChangeServerTB(1); Думал всё так просто?))))
                MainWindow.instance.Server_TB.Dispatcher.Invoke(
                new UpdateTBCallback(MainWindow.ChangeServerTB),
                new object[] { 1,server_ip });

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // New connection found
                    string client_ip = handler.RemoteEndPoint.ToString(); //Client's IP
                    MainWindow.instance.Client_TB.Dispatcher.Invoke(
                    new UpdateTBCallback(MainWindow.ChangeClientTB),
                    new object[] { 1, client_ip });

                    while (true)
                    {
                        GetInfo();
                        string message1 = cpu_name+" "+ cpu_cores+ " " + cpu_threads + " " + pc_name + " " + ram_total + " " + ram_available +"\n"; /// Make list
                        string message2= Convert.ToString(screen_height)+" "+Convert.ToString(screen_width)+"\n";
                        string message3 = Convert.ToString(ram_percent) +" "+ cpu_percent+"\n";
                        byte[] data = Encoding.Unicode.GetBytes(message1+message2+message3);
                        handler.Send(data);
                        Thread.Sleep(2000);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // Server is turned off
            }
        }
        

        public static void GetInfo()
        {
            // string DeviceID = mo["DeviceID"].ToString(); 
            WqlObjectQuery query = new WqlObjectQuery("Select * from Win32_Processor where DeviceID='CPU0'");
            ManagementObjectSearcher find = new ManagementObjectSearcher(query);
            foreach (ManagementObject mo in find.Get())
            {
                cpu_name = mo["Name"].ToString();
                cpu_cores = mo["NumberOfCores"].ToString();
                cpu_threads = mo["NumberOfLogicalProcessors"].ToString();
                cpu_percent = mo["LoadPercentage"].ToString();
                pc_name = mo["SystemName"].ToString();
            }

            query = new WqlObjectQuery("Select * from Win32_OperatingSystem");
            find = new ManagementObjectSearcher(query);
            foreach (ManagementObject mo in find.Get())
            {
                ram_total = mo["TotalVirtualMemorySize"].ToString();
                ram_available = mo["FreeVirtualMemory"].ToString();
                ram_percent = 1-(Convert.ToDouble(ram_available) / Convert.ToDouble(ram_total));
            }

            query = new WqlObjectQuery("Select * from Win32_DesktopMonitor where DeviceID='DesktopMonitor1'");
            find = new ManagementObjectSearcher(query);
            foreach (ManagementObject mo in find.Get())
            {
                screen_height = Convert.ToInt32(mo["ScreenHeight"]);
                screen_width = Convert.ToInt32(mo["ScreenWidth"]);
            }
        }
    }
}
