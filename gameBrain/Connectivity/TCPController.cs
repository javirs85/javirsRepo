using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace gameSystem
{
    public class TCPController
    {
        private StreamSocket TCPSocket;
        private DataWriter TCPWriter;
        private DataReader TCPReader;

        private volatile bool KeepOnReading = true;

        public bool IsConnected { get { return (TCPSocket != null); } }

        public async void Connect(string ip)
        {
            TCPSocket = new StreamSocket();
            gameBrain.Debug("trying to TCP connect " + new Windows.Networking.HostName(ip).CanonicalName + ":" + Utils.devicesTCPPort.ToString());
            await TCPSocket.ConnectAsync(new Windows.Networking.HostName(ip), Utils.devicesTCPPort.ToString());
            TCPWriter = new DataWriter(TCPSocket.OutputStream);
            TCPReader = new DataReader(TCPSocket.InputStream);


            StartListening();
            Send("message From gameBrain: welcoma aboard");
        }

        public event EventHandler<string> NewTCPMessage;

        public void Send(string msg)
        {
            TCPWriter.WriteString(msg);
        }

        private async Task StartListening()
        {
            string response;
            using (Stream inputStream = TCPSocket.InputStream.AsStreamForRead())
            {
                using (StreamReader streamReader = new StreamReader(inputStream))
                {
                    while (KeepOnReading)
                    {
                        response = await streamReader.ReadLineAsync();
                        if(response != null)
                            NewTCPMessage?.Invoke(this, response);
                        Task.Delay(1).Wait();
                    }
                }
            }
            TCPSocket.Dispose();
        }


        public void StopListening() => KeepOnReading = false;
       
        
    }

    class TCPServer
    {
        int baseTCPPort = 50100;
        StreamSocketListener streamSocketListener = null;

        public event EventHandler<string> newTCPmessage;
        private void Debug(string msg)
        {
            newTCPmessage?.Invoke(this, msg);
        }

        public async void Start()
        {
            try
            {
                streamSocketListener = new Windows.Networking.Sockets.StreamSocketListener();

                // The ConnectionReceived event is raised when connections are received.
                streamSocketListener.ConnectionReceived += StreamSocketListener_ConnectionReceived;

                // Start listening for incoming TCP connections on the specified port.
                //You can specify any port that's not currently in use.
                await streamSocketListener.BindServiceNameAsync(baseTCPPort.ToString());

                Debug("server is listening...");
            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
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
