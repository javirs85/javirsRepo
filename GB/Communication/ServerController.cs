using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Communication
{
    public class ServerController
    {
        public event EventHandler<string> newDebugMessage;
        public event EventHandler<TcpClient> newDeviceConnected;

        private TcpListener listener;
        static CancellationTokenSource TokenSource;
        public static CancellationToken Token;

        private static List<TcpClient> ConnectedClients;

        public ServerController()
        {
            Debug("TCP init  called");

            ConnectedClients = new List<TcpClient>();
            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;
            listener = new TcpListener(IPAddress.Any, gameNet.baseTCPPort);

        }

        public async void Start()
        {
            await Task.Run(async () =>
            {
                try
                {
                    listener.Start();
                    Debug("TCP server started listening at:" + gameNet.GetLocalIp());

                    while (!Token.IsCancellationRequested)
                    {
                        if (Token.IsCancellationRequested)
                        {
                            listener.Stop();
                            Token.ThrowIfCancellationRequested();
                        }

                        var tcpClient = await listener.AcceptTcpClientAsync();
                        newDeviceConnected?.Invoke(this, tcpClient);
                    }
                }
                catch (Exception exc)
                {
                    Debug(exc.Message);
                    Token.ThrowIfCancellationRequested();
                }
            });
        }

        public void Stop()
        {
            TokenSource.Cancel();
        }

        private void Debug(string s)
        {
            newDebugMessage?.Invoke(this, s);
        }
    }
}
