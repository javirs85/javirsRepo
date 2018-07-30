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


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebServerApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        WebSocketNuget superSocket;

        public MainPage()
        {
            this.InitializeComponent();
            superSocket = new WebSocketNuget();
            superSocket.StartAll();
            WebSocketNuget.newDebugMessageFromSuperServer += Debug;
            WebSocketNuget.newMessageFromSocket += Debug;
        }

        private async void Debug(object o,  string msg)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                debug.Text += Environment.NewLine + msg;
            });
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            superSocket.Stop();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            superSocket.Send("testikitest");
        }
    }
}
