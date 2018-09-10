using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace gameSystem
{
    public class UDPController
    {

        public static void Debug1()
        {
            Message m = new Message();
            m.Id = 1;
            m.Name = "Imanes";
            m.Status = Utils.PuzzleStatus.unsolved;
            m.PuzleKind = Utils.PuzzleKinds.button;
            m.IPSender = "192.168.137.2";
            m.Details = "This is a fake puzzle from memory";
            m.data = null;
            m.msgType = Utils.MessageTypes.present;

            NewUDPmessageFromDevice?.Invoke(null, m);
        }


        public static void Debug2()
        {
            Message m = new Message();
            m.Id = 1;
            m.Name = "Imanes";
            m.Status = Utils.PuzzleStatus.unsolved;
            m.PuzleKind = Utils.PuzzleKinds.button;
            m.IPSender = "192.168.137.2";
            m.Details = "They moved some magnets!";
            m.data = null;
            m.msgType = Utils.MessageTypes.update;

            NewUDPmessageFromDevice?.Invoke(null, m);
        }

        private DatagramSocket UDPListener = null;


        public static event EventHandler<Message> NewUDPmessageFromDevice;

        public UDPController()
        {
            UDPListener = new Windows.Networking.Sockets.DatagramSocket();

            //TODO: Add to the broadcast team
            //UDPListener.JoinMulticastGroup()
        }
        

        public async void StartListening()
        {
            try
            {
                await UDPListener.BindServiceNameAsync(Utils.baseUDPPort.ToString());
                UDPListener.MessageReceived += UDPListener_MessageReceived;
            }catch(Exception e)
            {
                gameBrain.Debug(e.Message);
            }
        }
        private async void UDPListener_MessageReceived(Windows.Networking.Sockets.DatagramSocket sender, Windows.Networking.Sockets.DatagramSocketMessageReceivedEventArgs args)
        {
            try
            {
                string request = "";

                using (var streamReader = new StreamReader(args.GetDataStream().AsStreamForRead()))
                {
                    request = await streamReader.ReadLineAsync();
                }

                Message m = Message.Deserialize(request);

                NewUDPmessageFromDevice?.Invoke(this, m);

            }
            catch (Exception e)
            {
                ;
            }
        }


        public static async void Send(string str, string ip = "192.168.1.40", bool isBroadcast = false)
        {
            try
            {
                using (var socket = new DatagramSocket())
                {
                    if(isBroadcast)
                        socket.Control.MulticastOnly = false;

                    var hn = new Windows.Networking.HostName(ip);
                    using (Stream outStream = (await socket.GetOutputStreamAsync(hn, Utils.devicesUDPPort.ToString())).AsStreamForWrite())
                    {
                        using (var streamWriter = new StreamWriter(outStream))
                        {
                            await streamWriter.WriteAsync(str);
                            await streamWriter.FlushAsync();
                        }
                    }
                }
                 gameBrain.Debug("message sent via UDP: " + str + " @" + ip + ":" + Utils.devicesUDPPort + "Broadcast: "+ isBroadcast);
            }
            catch(Exception e)
            {
                gameBrain.Debug(e.Message);
            }

        }

        public static void SendBroadcast(string str)
        {
            //var BroadcastAddress = GetBroadastAddress(IPAddress.Parse(Utils.GetLocalIp()));
            Send(str, "255.255.255.255", true);
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