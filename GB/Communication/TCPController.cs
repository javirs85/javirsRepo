using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communication
{
    public class TCPController
    {
        public event EventHandler<string> newDebugMessage;
        public event EventHandler<string> newMessageFromServer;
        public event EventHandler clientDisconnected;

        private TcpClient client;

        public async void ListenToClient(TcpClient tcpClient)
        {
            await Task.Run(() =>
            {
                try
                {
                    client = tcpClient;

                    bool Stop = false;
                    byte[] response = new byte[1024];
                    while (!ServerController.Token.IsCancellationRequested && !Stop)
                    {
                        Array.Clear(response, 0, response.Length);
                        int bytes = tcpClient.Client.Receive(response);
                        string data = Encoding.UTF8.GetString(response);
                        if (bytes == 0)
                        {
                            Stop = true;
                            ServerController.Token.ThrowIfCancellationRequested();
                        }
                        else
                            ProcesNewMessage(data);
                    }
                    if (ServerController.Token.IsCancellationRequested)
                    {
                        tcpClient.Close();
                        ServerController.Token.ThrowIfCancellationRequested();
                    }
                }
                catch (Exception exc)
                {
                    tcpClient.Close();

                    Debug(exc.Message);
                    ServerController.Token.ThrowIfCancellationRequested();
                }

            }, ServerController.Token).ContinueWith(
                (result) =>
                {
                    clientDisconnected?.Invoke(this, EventArgs.Empty);
                });
        }

        public void Send(string s)
        {
            var array = Encoding.UTF8.GetBytes(s);
            client.GetStream().WriteAsync(array, 0, array.Length);
        }

        private void Debug(string message) => newDebugMessage?.Invoke(this, message);

        private void ProcesNewMessage(string data)
        {
            var msgs = SplitIntoMessages(data);
            foreach (var msg in msgs)
                newMessageFromServer?.Invoke(this, msg);
            
        }

        private List<string> SplitIntoMessages(string source)
        {
            var toReturn = new List<string>();

            int init = source.IndexOf("{\"Id");
            int end = source.IndexOf("{\"Id", init+1);

            while (end > -1)
            {
                var str = source.Substring(init, (end - init));
                toReturn.Add(str);

                init = end;
                end = source.IndexOf("{\"Id", init + 1);
            }
            toReturn.Add(source.Substring(init, source.Length - init));

            return toReturn;
        }
    }
}
