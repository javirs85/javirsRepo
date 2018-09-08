using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace gameBrain
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        WebController webServer = null;

        public MainPage()
        {
            this.InitializeComponent();
            Debug(null," ");
            webServer = new WebController();
            WebController.newDebugMessageFromSuperServer += Debug;
            WebController.newMessageFromSocket += ProcessNewMessageFromDevice;
            webServer.StartAll();


            UDPController Udp = new UDPController();
            UDPController.newUDPmessage += Debug;
            UDPController.newDeviceAppeared += UDPController_newDeviceAppeared;
            Udp.StartListening();

            UDPController.SendBroadcast("SHOWUP");

            /*Puzzle local = new Puzzle("127.0.0.1", Utils.PuzzleKinds.button);
            local.Reset();
            */
        }

        private void UDPController_newDeviceAppeared(object sender, Message e)
        {
            Puzzle p = new Puzzle(e.IPSender, e.PuzleKind);
            webServer.Send(e.Serialize());
        }

        private void ProcessNewMessageFromDevice(object sender, string e)
        {
            if (e == "discoveryRequest")
                UDPController.SendBroadcast("ShowUp");
            Debug(null, "Website requested:" + e);
        }

        public void Debug(string str) { Debug(null, str); }

        public async void Debug(object o, string msg)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                debugTB.Text += msg + Environment.NewLine;
                webServer.Send(msg);
            });

        }

        private void broadcast_Click(object sender, RoutedEventArgs e)
        {
            UDPController.SendBroadcast("SHOWUP");
        }
    }
}
