using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using static MobileClient.MainPage;

namespace MobileClient
{
    class Program
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8005; // порт сервера
        static string address = "192.168.1.68"; // адрес сервера
        static string cpu_name, cpu_cores, cpu_threads, cpu_percent, pc_name, ram_total, ram_available, ram_percent;
        static string screen_height, screen_width;
        internal async static void Start()
        {
            //try
            //{
                int i = 0;
                //while (true)
                //{
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
                //Console.Write("Введите сообщение:");
                //string message = Convert.ToString(i);
                //byte[] data = Encoding.Unicode.GetBytes(message);
                //socket.Send(data);
                //i++;
                //Thread.Sleep(2000);
                //}
                //// получаем ответ
                while (true)
                {
                    byte[] data = new byte[512]; // буфер для ответа
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байт

                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        string info = builder.ToString();
                        string[] infos = info.Split(',').ToArray();
                        changeLabels(infos);
                        await Task.Delay(2000);
                    }
                    while (socket.Available > 0);
                }
                //
                //// закрываем сокет
                //socket.Shutdown(SocketShutdown.Both);
                //socket.Close();
            //}
            //catch (Exception ex)
            //{
            //}
        }
    }
}