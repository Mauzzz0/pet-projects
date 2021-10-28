using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using static MobileClient.MainPage;

namespace MobileClient
{
    class Program
    {
        static int port = 8005;
        static internal string address;
        static string cpu_name, cpu_cores, cpu_threads, cpu_percent, pc_name, ram_total, ram_available, ram_percent;
        static string screen_height, screen_width;
        static Socket socket;
        
        internal async static void Start(int x = 1)
        {
            try
            {
                if (x == 0) { socket.Close(); return; }
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);

                while (true)
                {
                    byte[] data = new byte[512]; // буфер для сообщения
                    StringBuilder builder = new StringBuilder();
                    int bytes;
                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        string info = builder.ToString();
                        string[] infos = info.Split(',').ToArray();
                        changeLabels(infos);
                        await Task.Delay(500);
                    }
                    while (socket.Available > 0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        internal static void Close()
        {
            Start(0);
        }

        static public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);
        }
    }
}