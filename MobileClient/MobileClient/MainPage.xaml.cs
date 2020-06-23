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
        static bool isConnected = false;
        public MainPage()
        {
            instance = this;
            InitializeComponent();
        }

        private async void ButtonConnect_Click(object sender, EventArgs e)
        {
            if (isConnected == false)
            {
                Program.Start();
                //Thread thrd = new Thread(Program.Start);
                //thrd.Start();
                //label1.Text = "async method started";
                //await Task.Delay(1000); // example purpose only
                //label1.Text = "1 second passed";
                //await Task.Delay(2000);
                //label1.Text = "2 more seconds passed";
                isConnected = true;
            }
        }

        internal static void changeLabels(string[] infos)
        {
            string cpu_name = infos[0];
            string cpu_cores = infos[1];
            string cpu_threads = infos[2];
            string pc_name = infos[3];
            string ram_total = infos[4];
            string ram_available = infos[5];
            string screen_height = infos[6];
            string screen_width = infos[7];
            string ram_percent = infos[8];
            string cpu_percent = infos[9];

            instance.label1.Text =  ram_percent;
            instance.Cpu_name.Text =  cpu_name;
            instance.Cpu_cores.Text = cpu_cores;
            instance.Cpu_threads.Text = cpu_threads;
            instance.Cpu_percent.Text = cpu_percent;
            instance.Pc_name.Text = pc_name;
            instance.Ram_Available.Text = ram_available;
            instance.Ram_total.Text = ram_total;
            instance.Ram_percent.Text = ram_percent;
            instance.Screen_height.Text = screen_height;
            instance.Screen_width.Text = screen_width;
        }
    }
}
