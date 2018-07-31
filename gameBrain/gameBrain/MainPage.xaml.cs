using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

            webServer = new WebController();
            WebController.NewDebugMessage += Debug;
            WebController.NewFrameToProcess += ProcessNewFrame;
            webServer.StartAll();
        }

        private void ProcessNewFrame(object sender, string e)
        {
            Debug(sender,"socket said: "+e);
            webServer.SendBroadcastMessage("received!");
        }

        public async void Debug(object sender, string msg)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                debugTB.Text += msg + Environment.NewLine;
            });

        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            webServer.SendBroadcastMessage("This is a test");
        }
    }
}
