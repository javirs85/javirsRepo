using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;

namespace gameBrain
{
    class WebServer
    {
        public event EventHandler<string> newDebugMessage;
        public int WebServerPort = 8080;

        private const uint BufferSize = 8192;
        private StreamSocketListener listener = null;

        public WebServer(int port)
        {
            WebServerPort = port;
        }

        public async void Start()
        {
            listener = new StreamSocketListener();
            listener.ConnectionReceived += Listener_ConnectionReceived;
            var host = FindHostName();

            await listener.BindEndpointAsync(host, WebServerPort.ToString());
            //await listener.BindServiceNameAsync(WebServerPort.ToString());
            
            Debug("server open, listening at "+ host.CanonicalName + ":" + WebServerPort.ToString());

        }

        private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            string debug = "Inconmming connection ... ";
            var request = new StringBuilder();

            using (var input = args.Socket.InputStream)
            {
                var data = new byte[BufferSize];
                IBuffer buffer = data.AsBuffer();
                var dataRead = BufferSize;

                while (dataRead == BufferSize)
                {
                    await input.ReadAsync(
                         buffer, BufferSize, InputStreamOptions.Partial);
                    request.Append(Encoding.UTF8.GetString(
                                                  data, 0, data.Length));
                    dataRead = buffer.Length;
                }
            }

            string query = GetQuery(request);

            debug += "requested " + query + " ... ";

            if (query != "favicon.ico")
            {
                await ServeFile(args.Socket.OutputStream, query);
                debug += "file served !";
            }

            Debug(debug);
        }

        private async Task ServeFile(IOutputStream stream, string fileName)
        {
            using (var output = stream)
            {
                using (var response = output.AsStreamForWrite())
                {
                    StorageFile f = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///" + @"Assets/web/" + fileName));

                    string content = "";
                    using (var reader = new StreamReader(await f.OpenStreamForReadAsync()))
                    {
                        content = await reader.ReadToEndAsync();
                    }

                    var html = Encoding.UTF8.GetBytes(content);
                    using (var bodyStream = new MemoryStream(html))
                    {
                        var header = $"HTTP/1.1 200 OK\r\nContent-Length: {bodyStream.Length}\r\nConnection: close\r\n\r\n";
                        var headerArray = Encoding.UTF8.GetBytes(header);
                        await response.WriteAsync(headerArray,
                                                  0, headerArray.Length);
                        await bodyStream.CopyToAsync(response);
                        await response.FlushAsync();
                    }
                }
            }
        }

        internal async void Stop()
        {
            await listener.CancelIOAsync();
            listener.Dispose();
            Debug("Server closed. Not listening");
        }

        private static string GetQuery(StringBuilder request)
        {
            var requestLines = request.ToString().Split(' ');

            var url = requestLines.Length > 1
                              ? requestLines[1] : string.Empty;

            var uri = new Uri("http://localhost" + url);
            var item = uri.LocalPath.Substring(1);
            if (item == "") item = "index.html";

            return item;
        }

        private void Debug(string msg)
        {
            if (newDebugMessage != null) newDebugMessage(this, msg);
        }

        public static HostName FindHostName()
        {
            List<HostName> hostnamesList = new List<HostName>();

            var hostnames = NetworkInformation.GetHostNames();
            foreach (var hn in hostnames)
            {
                //IanaInterfaceType == 71 => Wifi
                //IanaInterfaceType == 6 => Ethernet (Emulator)
                if (hn.IPInformation != null &&
                    (hn.IPInformation.NetworkAdapter.IanaInterfaceType == 71
                    || hn.IPInformation.NetworkAdapter.IanaInterfaceType == 6))
                {
                    hostnamesList.Add(hn);
                }
            }

            if (hostnamesList.Count < 1)
            {
                return null;
            }
            else if (hostnamesList.Count == 1)
            {
                return hostnamesList[0];
            }
            else
            {
                return hostnamesList[hostnamesList.Count - 1];
            }
        }

        public static string FindIPAddress()
        {
            List<string> ipAddresses = new List<string>();

            var hostnames = NetworkInformation.GetHostNames();
            foreach (var hn in hostnames)
            {
                //IanaInterfaceType == 71 => Wifi
                //IanaInterfaceType == 6 => Ethernet (Emulator)
                if (hn.IPInformation != null &&
                    (hn.IPInformation.NetworkAdapter.IanaInterfaceType == 71
                    || hn.IPInformation.NetworkAdapter.IanaInterfaceType == 6))
                {
                    string ipAddress = hn.DisplayName;
                    ipAddresses.Add(ipAddress);
                }
            }

            if (ipAddresses.Count < 1)
            {
                return null;
            }
            else if (ipAddresses.Count == 1)
            {
                return ipAddresses[0];
            }
            else
            {
                return ipAddresses[ipAddresses.Count - 1];
            }
        }
    }
}
