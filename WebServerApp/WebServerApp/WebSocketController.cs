using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace gameBrain
{
    class WebSocketController
    {
        private bool keepAlive = false;

        int baseTCPPort = 8082;
        StreamSocketListener streamSocketListener = null;

        public event EventHandler<string> newWebSocketServermessage;
        private void Debug(string msg)
        {
            newWebSocketServermessage?.Invoke(this, msg);
        }

        public async void Start()
        {
            IotWeb.Server.SocketServer server = new IotWeb.Server.SocketServer(8082);
            //server.ConnectionRequested = new IotWeb.Common.ConnectionHandler()

            /*
            try
            {
                keepAlive = true;
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
            */
        }

        private async void StreamSocketListener_ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender, Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {
            string data;

            using (var streamReader = new StreamReader(args.Socket.InputStream.AsStreamForRead()))
            {
                while (keepAlive)
                {
                    try
                    {
                        data = "";
                        string line = "";
                        
                        do
                        {
                            line = await streamReader.ReadLineAsync();
                            data += line + Environment.NewLine;
                        } while (line != "");


                        if (data == null)
                            Stop();
                        else if (data != "")
                        {
                            if (new Regex("^GET").IsMatch(data))
                            {
                                const string eol = "\r\n"; // HTTP/1.1 defines the sequence CR LF as the end-of-line marker

                                Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + eol
                                    + "Connection: Upgrade" + eol
                                    + "Upgrade: websocket" + eol
                                    + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                                        SHA1.Create().ComputeHash(
                                            Encoding.UTF8.GetBytes(
                                                new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                            )
                                        )
                                    ) + eol
                                    + eol);

                                var stream = args.Socket.OutputStream.AsStreamForWrite();
                                await stream.WriteAsync(response, 0, response.Length);
                                await stream.FlushAsync();
                                Debug("WebSocket tried to conect");

                                Byte[] msg = Encoding.UTF8.GetBytes("message from server");
                                await stream.WriteAsync(msg, 0, msg.Length);

                            }
                            else
                                Debug(string.Format("webSocket said: \"{0}\"", data));

                        }
                    }
                    catch (Exception e)
                    {
                        Debug("other side was disconnected : e=" + e.Message);
                        keepAlive = false;
                    }

                    await Task.Delay(25);
                }
            }


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

        private string decode(Byte[] encoded)
        {
            Byte[] decoded = new Byte[3];
            Byte[] key = new Byte[4] { 61, 84, 35, 6};

            for (int i = 0; i < encoded.Length; i++)
            {
                decoded[i] = (Byte)(encoded[i] ^ key[i % 4]);
            }

            return Encoding.UTF8.GetString(decoded);
        }

        internal async void Stop()
        {
            keepAlive = false;
            await streamSocketListener.CancelIOAsync();
            streamSocketListener.Dispose();
            Debug("websocket disconnected");
        }
    }
}
