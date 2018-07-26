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
        int basePort = 60100;

        public MainPage()
        {
            this.InitializeComponent();
            StartUDPListener();
        }

        public async void StartUDPListener()
        {
            var UDPListener = new Windows.Networking.Sockets.DatagramSocket();
            UDPListener.JoinMulticastGroup()
            await UDPListener.BindServiceNameAsync(basePort.ToString());
            UDPListener.MessageReceived += UDPListener_MessageReceived;
        }

        private async void UDPListener_MessageReceived(Windows.Networking.Sockets.DatagramSocket sender, Windows.Networking.Sockets.DatagramSocketMessageReceivedEventArgs args)
        {
            string request = "";

            using (var streamReader = new StreamReader(args.GetDataStream().AsStreamForRead()))
            {
                request = await streamReader.ReadLineAsync();
            }

            Debug(string.Format("UDP: \"{0}\"", request));
        }


        public async void Start()
        {
            try
            {
                var streamSocketListener = new Windows.Networking.Sockets.StreamSocketListener();

                // The ConnectionReceived event is raised when connections are received.
                streamSocketListener.ConnectionReceived += this.StreamSocketListener_ConnectionReceived;

                // Start listening for incoming TCP connections on the specified port. You can specify any port that's not currently in use.
                await streamSocketListener.BindServiceNameAsync(basePort.ToString());

                Debug("server is listening...");
            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
        }

        public void Debug(string msg)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                debugTB.Text += msg + Environment.NewLine;
            });

        }

        private async void StreamSocketListener_ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender, Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {
            string request;
            using (var streamReader = new StreamReader(args.Socket.InputStream.AsStreamForRead()))
            {
                request = await streamReader.ReadLineAsync();
            }

            Debug(string.Format("server received the request: \"{0}\"", request));
            /*
                       // Echo the request back as the response.
            using (Stream outputStream = args.Socket.OutputStream.AsStreamForWrite())
            {
                using (var streamWriter = new StreamWriter(outputStream))
                {
                    await streamWriter.WriteLineAsync(request);
                    await streamWriter.FlushAsync();
                }
            }

            Debug(string.Format("server sent back the response: \"{0}\"", request));

            sender.Dispose();
            */
            Debug("server closed its socket");
        }

    }
}
