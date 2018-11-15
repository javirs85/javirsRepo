using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPComm
{
    public class TCPConnector : IDisposable
    {

        //client socket 
        private Socket workSocket = null;
        // Size of receive buffer.  
        private const int BufferSize = 1024;
        // Receive buffer.  
        private byte[] buffer = new byte[BufferSize];
        // Received data string.  
        private string data = "";

        public bool IsConnected
        {
            get { return workSocket.Connected; }
        }


        public event EventHandler<string> newRawMessage;
        public event EventHandler disconnected;
        public event EventHandler<Exception> newError;

        CancellationTokenSource source;


        public TCPConnector() { }

        public TCPConnector(Socket socket)
        {
            workSocket = socket;
        }

        public async void StartListening()
        {
            source = new CancellationTokenSource();

            await Task.Run(() =>
            {
                try
                {
                    while (!source.Token.IsCancellationRequested)
                    {
                        Array.Clear(buffer, 0, buffer.Length);
                        int numBytes = workSocket.Receive(buffer);
                        data = Encoding.UTF8.GetString(buffer, 0, numBytes);
                        newRawMessage?.Invoke(this, data);
                    }
                }
                catch (Exception e)
                {
                    if (e is SocketException)
                    {
                        var SocketError = e as SocketException;
                        if (SocketError.NativeErrorCode == 10053 || SocketError.NativeErrorCode == 10054) //the task was canceled byt the system.
                            disconnected.Invoke(this, EventArgs.Empty);
                        else
                            newError.Invoke(this, e);

                    }
                    else
                        newError.Invoke(this, e);
                }
            }, source.Token);
        }

        public void Stop()
        {
            source.Cancel();
            workSocket.Close();
        }

        public void Send(string str)
        {
            if (this.IsConnected)
                workSocket.Send(Encoding.UTF8.GetBytes(str));
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
