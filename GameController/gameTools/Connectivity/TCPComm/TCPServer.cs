using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCPComm
{
    public class TCPServer
    {
        private List<TCPConnector> ConnectedClients = new List<TCPConnector>();

        public int NumConnectedClients
        {
            get
            {
                return ConnectedClients.Count;
            }
        }

        public event EventHandler<TCPConnector> newDeviceConnected;
        public event EventHandler<TCPConnector> DeviceDisconnected;
        public event EventHandler<Exception> newError;
        public event EventHandler<string> newDebugMessage;

        public CancellationTokenSource TokenSource;
        public Socket listener;
        private int port;

        public bool IsRunning { get; set; }



        public TCPServer(int _port, bool AutoStart = true)
        {
            port = _port;
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (AutoStart) OpenServer();
        }

        public async void OpenServer()
        {
            TokenSource = new CancellationTokenSource();
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(Utils.GetLocalIp()), port);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                await Task.Run(() =>
                {
                    try
                    {
                        newDebugMessage(this, $"Server started accepting connections at {Utils.GetLocalIp().ToString()}:{port}");
                        IsRunning = true;
                        while (!TokenSource.Token.IsCancellationRequested)
                        {
                            var handler = listener.Accept();
                            var connectedItem = new TCPConnector(handler);
                            connectedItem.disconnected += (sender, arg) => {
                                var connector = sender as TCPConnector;
                                lock (ConnectedClients)
                                    ConnectedClients.Remove(connector);
                                DeviceDisconnected?.Invoke(this, connector);
                                connector.Dispose();
                            };
                            connectedItem.newError += (sender2, err2) => {
                                this.newError(this, err2);
                            };

                            connectedItem.StartListening();

                            lock (ConnectedClients)
                                ConnectedClients.Add(connectedItem);
                            newDeviceConnected?.Invoke(this, connectedItem);
                        }
                        IsRunning = false;
                    }
                    catch (Exception e)
                    {
                        IsRunning = false;
                        if (!((e as SocketException).NativeErrorCode == 10004)) //error code for "async task canceled"
                            newError?.Invoke(this, e);
                    }
                });
            }
            catch (Exception e)
            {
                newError?.Invoke(this, e);
            }
        }

        public void SendToAllConnectedDevices(string str)
        {
            lock (ConnectedClients)
            {
                foreach (var d in ConnectedClients)
                    d.Send(str);
            }
        }

        public void Stop()
        {
            listener.Close();
            TokenSource.Cancel();

            lock (ConnectedClients)
            {
                foreach (var client in ConnectedClients)
                    client.Stop();
            }
        }
    }

}
