using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace deviceSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int basePort = 60100;

        enum methods { UDP, TCP};

        public MainWindow()
        {
            InitializeComponent();
            methods method = methods.UDP;

            if (method == methods.UDP)
            {
                UdpClient udp = new UdpClient();

                string message = "I am a device";
                byte[] sendBytes4 = Encoding.ASCII.GetBytes(message);

                IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse("255.255.255.255"), basePort);
                udp.Send(sendBytes4, sendBytes4.Length, groupEP);
            }
            else
            {
                TCP tcpServer = new TCP("192.168.0.17");
                tcpServer.Send("testing quick TCP");
            }

        }
    }
}
