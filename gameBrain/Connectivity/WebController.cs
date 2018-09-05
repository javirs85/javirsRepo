using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IotWeb.Server;
using IotWeb.Common.Http;
using IotWeb.Common.Util;

namespace gameBrain
{
    public class WebController
    {
        HttpServer webServer;
        public static List<WebSocket> Sockets;
        public static event EventHandler<string> newDebugMessageFromSuperServer;
        public static event EventHandler<string> newMessageFromSocket;

        public void StartAll()
        {
            Sockets = new List<WebSocket>();

            webServer = new HttpServer(8006);
            webServer.AddHttpRequestHandler(
                "/",
                new HttpResourceHandler(
                    Utilities.GetContainingAssembly(typeof(WebController)),
                    "Resources.Site",
                    "index.html"
                )
            );
            webServer.AddWebSocketRequestHandler(
                "/sockets/",
                new WebSocketHandler()
            );

            webServer.Start();
            var Ip = Utils.GetLocalIp();
            OnDebugMessage(null, "Web server oppened at " + Ip + ":8006");
        }

        public void Send(string msg)
        {
            foreach (var socket in Sockets)
                socket.Send(msg);
        }

        public static void OnDebugMessage(WebSocket sender, string str)
        {
            newDebugMessageFromSuperServer?.Invoke(sender, str);
        }
        public static void OnMessageFromSocket(WebSocket sender, string str)
        {
            newMessageFromSocket?.Invoke(sender, str);
        }

        public void Stop() { webServer.Stop(); }
    }

    class WebSocketHandler : IWebSocketRequestHandler
    {
        public async void Connected(WebSocket socket)
        {
            WebController.Sockets.Add(socket);
            socket.DataReceived += Socket_DataReceived;

            var storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await storageFolder.CreateFileAsync("debug.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);
            var content = await Windows.Storage.FileIO.ReadTextAsync(file);
            
            WebController.OnDebugMessage(socket, "debug info: <br/>"+content);
        }

        private void Socket_DataReceived(WebSocket socket, string frame)
        {
            WebController.OnMessageFromSocket(socket, frame);
        }

        public bool WillAcceptRequest(string uri, string protocol)
        {
            return (uri.Length == 0) && (protocol == "brain");
        }
    }
}
