using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IotWeb.Server;
using IotWeb.Common.Http;
using IotWeb.Common.Util;

namespace gameSystem
{
    public class WebController
    {
        HttpServer webServer;
        public static List<WebSocket> Sockets;
        public static event EventHandler<string> NewMessageFromSocket;

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

            gameBrain.Debug("Web server oppened at " + Ip + ":8006", true);
        }

        public void Send(string msg)
        {
            foreach (var socket in Sockets)
                socket.Send(msg);
        }
        
        public static void OnMessageFromSocket(WebSocket sender, string str)
        {
            NewMessageFromSocket?.Invoke(sender, str);
        }

        public void Stop() { webServer.Stop(); }
    }

    class WebSocketHandler : IWebSocketRequestHandler
    {
        public void Connected(WebSocket socket)
        {
            WebController.Sockets.Add(socket);
            socket.DataReceived += Socket_DataReceived;
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
