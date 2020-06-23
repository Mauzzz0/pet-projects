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
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WirelessMonitoring
{
    class Program
    {
        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        static Socket handler;
        static Socket listenSocket;
        const int server_port = 8005;
        const string server_ip = "192.168.1.68";

        static string cpu_name, cpu_cores, cpu_threads, cpu_percent, pc_name, ram_total, ram_available, ram_percent;
        static string screen_height, screen_width;
        public delegate void UpdateTBCallback1(int key);
        public delegate void UpdateTBCallback2(int key,string text);
        
        static public void Start(object x = null)
        {
            try 
            {
                if (x != null) { handler.Close(); listenSocket.Close(); return; }
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(server_ip), server_port);
                listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                listenSocket.Bind(ipPoint);
                listenSocket.Listen(0);
                // Server is started
                //MainWindow.ChangeServerTB(1); Думал всё так просто?))))
                MainWindow.instance.Server_TB.Dispatcher.Invoke(
                new UpdateTBCallback2(MainWindow.ChangeServerTB),
                new object[] { 1,server_ip });

                while (true)
                {
                    handler = listenSocket.Accept();
                    // New connection found
                    // Receive message
                    Thread thrd = new Thread(ReceiveMessage);
                    thrd.Start();

                    string client_ip = handler.RemoteEndPoint.ToString(); //Client's IP
                    MainWindow.instance.Client_TB.Dispatcher.Invoke(
                    new UpdateTBCallback2(MainWindow.ChangeClientTB),
                    new object[] { 1, client_ip });

                    while (true)
                    {
                        GetInfo();
                        byte[] data;
                        string message1 = cpu_name+","+ cpu_cores+ "," + cpu_threads + "," + pc_name + "," + ram_total + "," + ram_available +","; /// Make list
                        string message2= screen_height+","+screen_width+",";
                        string message3 = ram_percent +","+ cpu_percent;
                        data = Encoding.Unicode.GetBytes(message1+message2+message3);
                        handler.Send(data);
                        Thread.Sleep(500);
                    }
                }
            }
            catch (SocketException)
            {
                MainWindow.instance.Client_TB.Dispatcher.Invoke(
                    new UpdateTBCallback1(MainWindow.ChangeClientTB),
                    new object[] { 0 });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // Server is turned off
                listenSocket.Close();
                handler.Close();

                MainWindow.instance.Client_TB.Dispatcher.Invoke(
                    new UpdateTBCallback1(MainWindow.ChangeClientTB),
                    new object[] { 0 });
                MainWindow.instance.Server_TB.Dispatcher.Invoke(
                    new UpdateTBCallback1(MainWindow.ChangeServerTB),
                    new object[] { 0 });
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
                ram_percent = Convert.ToString(1-(Convert.ToDouble(ram_available) / Convert.ToDouble(ram_total))).Replace(',','.');
            }

            query = new WqlObjectQuery("Select * from Win32_DesktopMonitor where DeviceID='DesktopMonitor1'");
            find = new ManagementObjectSearcher(query);
            foreach (ManagementObject mo in find.Get())
            {
                screen_height = Convert.ToString(mo["ScreenHeight"]);
                screen_width = Convert.ToString(mo["ScreenWidth"]);
            }
        }
        static public void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных
                    bytes = handler.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    string message = builder.ToString();
                    switch (message)
                    {
                        case "SLEEP":
                            goSleep();
                            break;
                        case "REBOOT":
                            goReboot();
                            break;
                        case "POWER":
                            goPower();
                            break;
                    }
                }
                catch { }
            }
        }
        static void goSleep()
        {
            try
            {
                handler.Close();
                listenSocket.Close();

            }
            catch { }
            SetSuspendState(false, true, true);
            Environment.Exit(0);
        }
        static void goReboot()
        {
            try
            {
                handler.Close();
                listenSocket.Close();

            }
            catch { }
            Process.Start("shutdown","/r /t 0");
            Environment.Exit(0);
        }
        static void goPower()
        {
            try
            {
                handler.Close();
                listenSocket.Close();

            }
            catch { }
            Process.Start("shutdown", "/s /t 0");
            Environment.Exit(0);
        }
    }
}
