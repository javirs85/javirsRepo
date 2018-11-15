using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPComm
{
    public class TCPClient : IDisposable
    {
        /// <summary>
        /// new raw json comming from the main server
        /// </summary>
        public event EventHandler<string> newJSONMessage;

        /// <summary>
        /// New internal debug message. Ignore for production
        /// </summary>
        public event EventHandler<string> newDebugMessageFromTCPClient;

        /// <summary>
        /// new Error (Exception)
        /// </summary>
        public event EventHandler<Exception> newError;

        /// <summary>
        /// true if connected to the mainServer
        /// </summary>
        public bool IsListening { get; private set; }

        private Socket client;
        private CancellationTokenSource source;

        private int port;


        /// <summary>
        /// Creates AND CONNECTES the client to an existing server at the specified port. IP is hardcoded to be 127.0.0.1
        /// </summary>
        /// <param name="_port">port where the server is running</param>
        public TCPClient(int _port)
        {
            port = _port;
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            source = new CancellationTokenSource();
            Start();
        }

        private async void Start()
        {
            try
            {
                var remoteEndPoint = new IPEndPoint(
                                    new IPAddress(new byte[] { 127, 0, 0, 1 }),
                                    port);

                await client.ConnectTaskAsync(remoteEndPoint);

                source = new CancellationTokenSource();

                await Task.Run(() =>
                {
                    int BufferSize = 1024;
                    byte[] buffer = new byte[BufferSize];
                    string data = "";
                    try
                    {
                        IsListening = true;
                        while (!source.Token.IsCancellationRequested)
                        {
                            Array.Clear(buffer, 0, buffer.Length);
                            int numBytes = client.Receive(buffer);
                            data = Encoding.UTF8.GetString(buffer, 0, numBytes);
                            if (data != "")
                                processNewMessage(data);
                        }
                        IsListening = false;
                    }
                    catch (Exception e)
                    {
                        if (e is SocketException)
                        {
                            var SocketError = e as SocketException;
                            if (SocketError.NativeErrorCode != 10053 && SocketError.NativeErrorCode != 10054) //the task was canceled byt the system.
                                newError(this, e);
                        }
                        else
                            newError(this, e);
                    }
                    finally
                    {
                        IsListening = false;
                    }
                }, source.Token);
            }
            catch (Exception e)
            {
                newError.Invoke(this, e);
            }
        }

        private void processNewMessage(string data)
        {
            newJSONMessage?.Invoke(this, data);
        }

        /// <summary>
        /// Closes the internal TCPClient and disconnects from the main server
        /// </summary>
        public void Disconnect()
        {
            client.Close();
        }

        private void Debug(string str) => newDebugMessageFromTCPClient?.Invoke(this, str);

        /// <summary>
        /// Sends the string to the main server (must be already connected)
        /// </summary>
        /// <param name="msg"></param>
        public void Send(string msg)
        {
            try
            {
                if (client.Connected == false)
                    throw new Exception("Client is not connected to any socket");
                var array = Encoding.UTF8.GetBytes(msg);
                var details = new SocketAsyncEventArgs();
                details.SetBuffer(array, 0, array.Length);
                client.SendAsync(details);
            }
            catch (Exception e)
            {
                newError(this, e);
            }
        }

        public void Dispose()
        {
            this.Disconnect();
        }
    }

    /// <summary>
    /// extension method used for facility. please ignore.
    /// </summary>
    public static class Extensions
    {
        public static Task ConnectTaskAsync(this Socket socket, EndPoint remoteEP)
        {
            return Task.Factory.FromAsync(socket.BeginConnect, socket.EndConnect, remoteEP, null);
        }
    }
}
