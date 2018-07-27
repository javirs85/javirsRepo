using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace gameBrain
{
    class UDPController
    {
        int baseUDPPort = 60100;
        private DatagramSocket UDPListener = null;

        public UDPController()
        {
            UDPListener = new Windows.Networking.Sockets.DatagramSocket();

            //TODO: Add to the broadcast team
            //UDPListener.JoinMulticastGroup()
        }

        public async void StartListening()
        {
            await UDPListener.BindServiceNameAsync(baseUDPPort.ToString());
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

        public event EventHandler<string> newUDPmessage;
        private void Debug(string msg)
        {
            newUDPmessage?.Invoke(this, msg);
        }
    }
}
