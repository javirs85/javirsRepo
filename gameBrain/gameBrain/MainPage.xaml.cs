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

namespace gameSystem
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private static MainPage myWindow;

        public MainPage()
        {
            this.InitializeComponent();
            myWindow = this;
            DebugOnRaspiUI(null," ");
            gameBrain.NewDebugMsgForUI += DebugOnRaspiUI;

            /*Puzzle local = new Puzzle("127.0.0.1", Utils.PuzzleKinds.button);
            local.Reset();
            */

            gameBrain.Start();
        }


        public static async void DebugOnRaspiUI(object o, string msg)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                MainPage.myWindow.debugTB.Text += msg + Environment.NewLine;                
            });
        }

        private void Debug1_Click(object sender, RoutedEventArgs e)
        {
            UDPController.Debug1();
        }

        private void Debug2_Click(object sender, RoutedEventArgs e)
        {
            UDPController.Debug2();
        }
    }
}
