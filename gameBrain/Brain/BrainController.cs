using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameBrain
{
    public class BrainController
    {
        public event EventHandler<string> newDebugMessage;

        private WebController webServer = null;
        private UDPController UDP;


        public void StartAllConnnectivity()
        {
            StartWebServer();
            StartUDPListener();
        }

        public void StartUDPListener()
        {
            UDP = new UDPController();
            UDP.newUDPmessage += (sender, msg) => {
                Debug(sender, "new UDP message: " + msg);
            };
            UDP.StartListening();
        }

        public void StartWebServer(int port = 8006)
        {
            webServer = new WebController();
            WebController.NewDebugMessage += Debug;
            WebController.NewFrameToProcess += ProccessNewFrame;
            webServer.StartAll(port);
        }

        public void Test()
        {
            Message msg = new Message() {MessageType= Utils.MessageTypes.showup };
            string str = msg.Serialize();
            UDPController.SendBroadcast(str);
        }

        private void ProccessNewFrame(object sender, string e)
        {
            Debug(sender, "socket said: " + e);
            webServer.SendBroadcastMessage("received!");
        }

        private void Debug(string s) { Debug(null, s); }

        private void Debug(object sender, string s) { newDebugMessage?.Invoke(sender, s); }
    }
}
