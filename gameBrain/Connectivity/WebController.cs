using IotWeb.Common.Http;
using IotWeb.Common.Util;
using IotWeb.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace gameBrain
{
    public class WebController
    {
        public static event EventHandler<string> NewDebugMessage;
        public static event EventHandler<string> NewFrameToProcess;

        private static List<WebSocket> connectedSockets;
        private BaseHttpServer m_server;
        private int port;

        public void StartAll(int _port = 8006)
        {
            port = _port;
            m_server = new HttpServer(port);
            m_server.AddHttpRequestHandler(
                "/",
                new HttpResourceHandler(
                    Utilities.GetContainingAssembly(typeof(WebController)),
                    "Resources.Site",
                    "index.html"
                )
            );
            m_server.AddWebSocketRequestHandler(
                "/sockets/",
                new WebSocketHandler()
            );

            connectedSockets = new List<WebSocket>();
            m_server.Start();
            
            Debug("Started at "+ Utils.GetLocalIp() + ":" +port);
        }

        public void Stop()
        {
            m_server.Stop();
            Debug("Stopped");
        }

        public void Debug(string str)
        {
            NewDebugMessage?.Invoke(this, "webserver : "+ str);
        }

        public void SendBroadcastMessage(string str)
        {
            foreach (var socket in connectedSockets)
                socket.Send(str);
        }

       

        class WebSocketHandler : IWebSocketRequestHandler
        {
            public bool WillAcceptRequest(string uri, string protocol)
            {
                return (uri.Length == 0) && (protocol == "brain");
            }

            public void Connected(WebSocket socket)
            {
                socket.DataReceived += OnDataReceived;
                WebController.NewDebugMessage(this,"web connected to the websocket");
                connectedSockets.Add(socket);
            }

            void OnDataReceived(WebSocket socket, string frame)
            {
                NewFrameToProcess?.Invoke(socket,frame);
            }
        }


    }
}
