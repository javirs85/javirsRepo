using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace gameBrain
{
    public class UDPController
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

        public static async void Send(string str, string ip = "192.168.1.40")
        {
            using (var socket = new DatagramSocket())
            {
                var hn = new Windows.Networking.HostName(ip);
                using (Stream outStream = (await socket.GetOutputStreamAsync(hn, "12345")).AsStreamForWrite())
                {
                    using (var streamWriter = new StreamWriter(outStream))
                    {
                        await streamWriter.WriteAsync(str);
                        await streamWriter.FlushAsync();
                    }
                }
            }                
        }

        public static void SendBroadcast(string str)
        {
            var BroadcastAddress = GetBroadastAddress(IPAddress.Parse(Utils.GetLocalIp()));
            Send(str, BroadcastAddress.ToString());
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

        /// <summary>
        /// this method helps with getting the subnet mask for the network from the device IP address:
        /// </summary>
        /// <param name="hostAddress">current local address</param>
        /// <returns></returns>
        private static IPAddress GetSubnetMask(IPAddress hostAddress)
        {
            var addressBytes = hostAddress.GetAddressBytes();
            if (addressBytes[0] >= 1 && addressBytes[0] <= 126)
                return IPAddress.Parse("255.0.0.0");
            else if (addressBytes[0] >= 128 && addressBytes[0] <= 191)
                return IPAddress.Parse("255.255.255.0");
            else if (addressBytes[0] >= 192 && addressBytes[0] <= 223)
                return IPAddress.Parse("255.255.255.0");
            else
                throw new ArgumentOutOfRangeException();
        }
        

        /// <summary>
        /// This method is the one that will give the Directed broadcast address:
        /// </summary>
        /// <param name="hostIPAddress"></param>
        /// <returns></returns>
        private static IPAddress GetBroadastAddress(IPAddress hostIPAddress)
        {
            var subnetAddress = GetSubnetMask(hostIPAddress);
            var deviceAddressBytes = hostIPAddress.GetAddressBytes();
            var subnetAddressBytes = subnetAddress.GetAddressBytes();
            if (deviceAddressBytes.Length != subnetAddressBytes.Length)
                throw new ArgumentOutOfRangeException();
            var broadcastAddressBytes = new byte[deviceAddressBytes.Length];
            for (var i = 0; i < broadcastAddressBytes.Length; i++)
                broadcastAddressBytes[i] = (byte)(deviceAddressBytes[i] | subnetAddressBytes[i] ^ 255);
            return new IPAddress(broadcastAddressBytes);
        }
    }
}
