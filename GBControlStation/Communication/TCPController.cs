using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Communication
{
    public class TCPController
    {
        public event EventHandler<string> newDebugMessage;
        private TcpListener listener;
        static CancellationTokenSource TokenSource;
        static CancellationToken  Token;

        private static List<TcpClient> ConnectedClients;

        public TCPController()
        {
            Debug("TCP init  called");

            ConnectedClients = new List<TcpClient>();
            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;
            listener = new TcpListener(IPAddress.Any,50100);

        }

        public async void Start()
        {
            await Task.Run(async () =>
            {
                try
                {
                    listener.Start();
                    Debug("TCP server started listening at:"+listener.Server.LocalEndPoint.ToString());

                    while (!Token.IsCancellationRequested)
                    {
                        if (Token.IsCancellationRequested)
                        {
                            listener.Stop();
                            Token.ThrowIfCancellationRequested();
                        }

                        var tcpClient = await listener.AcceptTcpClientAsync();
                        ConnectedClients.Add(tcpClient);
                        Debug("TCP Client connected ("+ConnectedClients.Count+")");
                        await Task.Run(() => {
                            ListenToClient(tcpClient);
                        }, Token).ContinueWith(
                            (result)=> {
                                ConnectedClients.Remove(tcpClient);
                                Debug("TCP client disconnected ("+ConnectedClients.Count+")");
                            });
                    }
                }
                catch (Exception exc)
                {
                    Debug(exc.Message);
                    Token.ThrowIfCancellationRequested();
                }
            });
        }

        private void ListenToClient(TcpClient tcpClient)
        {
            try
            {
                bool Stop = false;
                byte[] response = new byte[1024];
                while (!Token.IsCancellationRequested && !Stop)
                {
                    Array.Clear(response, 0, response.Length);
                    tcpClient.Client.Receive(response);
                    String data = Encoding.UTF8.GetString(response);
                    if (data[0] == data[1] && data[1] == data[2])
                    {
                        Stop = true;
                        Token.ThrowIfCancellationRequested();
                    }
                    else
                        procesNewMessage(data);
                }
                if (Token.IsCancellationRequested)
                {
                    tcpClient.Close();
                    Token.ThrowIfCancellationRequested();
                }
            }
            catch (Exception exc)
            {

                tcpClient.Close();

                Debug(exc.Message);
                Token.ThrowIfCancellationRequested();
            }
        }

        private void procesNewMessage(string data)
        {
            
        }

        private void ClientDisconnected(TcpClient Client)
        {
            ConnectedClients.Remove(Client);
            Debug("client disconnected ("+ConnectedClients.Count+")");
        }

        private void ListenCompleted(object sender, SocketAsyncEventArgs e)
        {
            string data = Encoding.UTF8.GetString(e.Buffer);
            Debug(data);
        }

        private void Args_Completed(object sender, SocketAsyncEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Stop()
        {
            TokenSource.Cancel();
        }

        private void Debug(string s)
        {
            newDebugMessage?.Invoke(this, s);
            Console.WriteLine("testing debug from TCP");
        }
    }
}
