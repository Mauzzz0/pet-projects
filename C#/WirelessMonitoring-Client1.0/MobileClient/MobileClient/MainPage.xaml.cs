using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using static MobileClient.MainPage;

namespace MobileClient
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        static internal MainPage instance { get; set; }
        public MainPage()
        {
            instance = this;
            InitializeComponent();
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
                Program.Start();
                //Thread thrd = new Thread(Program.Start);
                //thrd.Start();
                //label1.Text = "async method started";
                //await Task.Delay(1000); // example purpose only
                //label1.Text = "1 second passed";
                //await Task.Delay(2000);
                //label1.Text = "2 more seconds passed";
                ButtonDisconnect.IsEnabled = true;
                ButtonConnect.IsEnabled = false;
        }

        private void ButtonDisconnect_Click(object sender, EventArgs e)
        {
            Program.Close();
            ButtonDisconnect.IsEnabled = false;
        }

        private void Power_Click(object sender, EventArgs e)
        {
            Program.SendMessage("POWER");
        }

        private void Reboot_Click(object sender, EventArgs e)
        {
            Program.SendMessage("REBOOT");
        }

        private void Sleep_Click(object sender, EventArgs e)
        {
            Program.SendMessage("SLEEP");
        }

        private void InputIP(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            IPinput.IsEnabled = false;
            ButtonConnect.IsEnabled = true;
            Program.address = IPinput.Text;
        }

        internal static void changeLabels(string[] infos)
        {
            string cpu_name = infos[0];
            string cpu_cores = infos[1]+" Cores";
            string cpu_threads = infos[2]+" Threads";
            string pc_name = infos[3];
            string _ram_total = infos[4];
            string _ram_available = infos[5];
            string screen_height = infos[6]+" vert";
            string screen_width = infos[7]+" hor";
            string ram_percent = infos[8].Replace('.',',');
            string cpu_percent = infos[9];

            double ram_total = Convert.ToDouble(Convert.ToInt64(_ram_total) / (double)1024 / 1024);
            double ram_available = Convert.ToDouble(Convert.ToInt64(_ram_available) / (double)1024 / 1024);

            //instance.label1.Text = Convert.ToString(Math.Round(Convert.ToDouble(ram_percent), 2)) + "%";
            instance.Cpu_name.Text =  cpu_name;
            instance.Cpu_cores.Text = cpu_cores;
            instance.Cpu_threads.Text = cpu_threads;
            instance.Cpu_percent.Text = cpu_percent+"%";
            instance.Pc_name.Text = pc_name;
            instance.Ram_Available.Text = Convert.ToString(Math.Round(ram_available,2)+ " Gbytes");
            instance.Ram_total.Text = Convert.ToString(Math.Round(ram_total,2)+ " Gbytes");
            instance.Ram_percent.Text = Convert.ToString(Math.Round(Convert.ToDouble(ram_percent)*100, 2)) + "%";
            instance.Screen_height.Text = screen_height;
            instance.Screen_width.Text = screen_width;
        }
    }
}
