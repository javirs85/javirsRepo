using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace gameBrain
{
    class WebSocketController
    {
        MessageWebSocket server 

        public WebSocketController()
        {
            var server = new MessageWebSocket();
           
        }

        public async void Connect()
        {
            await server.ConnectAsync(new Uri("wss://www.contoso.com/mywebservice"));
        }
    }
}
