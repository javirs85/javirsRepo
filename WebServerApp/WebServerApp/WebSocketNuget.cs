using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IotWeb.Server;
using IotWeb.Common.Http;
using IotWeb.Common.Util;

namespace WebServerApp
{
    class WebSocketNuget
    {
        HttpServer webServer;
        public static List<WebSocket> Sockets;
        public static event EventHandler<string> newDebugMessageFromSuperServer;
        public static event EventHandler<string> newMessageFromSocket;

        public void StartAll()
        {
            Sockets = new List<WebSocket>();
            webServer = new HttpServer(8000);
            webServer.AddHttpRequestHandler(
                "/",
                new HttpResourceHandler(
                    Utilities.GetContainingAssembly(typeof(WebSocketNuget)),
                    "Resources.Site",
                    "index.html"
                )
            );
            webServer.AddWebSocketRequestHandler(
                "/sockets/",
                new WebSocketHandler()
            );

            webServer.Start();
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
        public void Connected(WebSocket socket)
        {
            WebSocketNuget.Sockets.Add(socket);
            socket.DataReceived += Socket_DataReceived;
            WebSocketNuget.OnDebugMessage(socket, "Socket connected");
        }

        private void Socket_DataReceived(WebSocket socket, string frame)
        {
            WebSocketNuget.OnMessageFromSocket(socket, frame);
        }

        public bool WillAcceptRequest(string uri, string protocol)
        {
            return (uri.Length == 0) && (protocol == "brain");
        }
    }
}
