using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using static WirelessMonitoring.Program;

namespace WirelessMonitoring
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            instance = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread thrd = new Thread(new ParameterizedThreadStart(Start));
            thrd.Start();
            ButtonON.IsEnabled = false;
            ButtonOFF.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Start(1);
            ButtonOFF.IsEnabled = false;
            ChangeServerTB(0);
            ChangeClientTB(0);
        }
        
        static internal void ChangeServerTB(int key)
        {
            switch (key)
            {
                case 0:
                    instance.Server_TB.Foreground = Brushes.Red;
                    break;
                case 1:
                    instance.Server_TB.Foreground = Brushes.Green;
                    break;
            }
        }
        static internal void ChangeServerTB(int key, string text)
        {
            switch (key)
            {
                case 0:
                    instance.Server_TB.Foreground = Brushes.Red;
                    instance.Server_TB.Text = text;
                    break;
                case 1:
                    instance.Server_TB.Foreground = Brushes.Green;
                    instance.Server_TB.Text = text;
                    break;
            }
        }
        static internal void ChangeClientTB(int key)
        {
            switch (key)
            {
                case 0:
                    instance.Client_TB.Foreground = Brushes.Red;
                    break;
                case 1:
                    instance.Client_TB.Foreground = Brushes.Green;
                    break;
            }
        }
        static internal void ChangeClientTB(int key, string text)
        {
            switch (key)
            {
                case 0:
                    instance.Client_TB.Foreground = Brushes.Red;
                    instance.Client_TB.Text = text;
                    break;
                case 1:
                    instance.Client_TB.Foreground = Brushes.Green;
                    instance.Client_TB.Text = text;
                    break;
            }
        }
    }
}
