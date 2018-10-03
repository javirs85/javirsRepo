using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communication;

namespace GBControlStation
{
    public class GameBrain
    {
        public event EventHandler<string> newMessageToUI;
        private TCPController TCP;

        public static CancellationTokenSource TCPTokenSource;
        public static CancellationToken TCPcancelToken;


        internal void Init()
        {
            TCPTokenSource = new CancellationTokenSource();
            TCPcancelToken = TCPTokenSource.Token;

            DebugMessage("initializing gameBrain");
            TCP = new TCPController();
            TCP.newDebugMessage += Debug;
            TCP.Start();
        }

        private void Debug(object sender, string e)
        {
            DebugMessage(e);
        }

        public void DebugMessage(string s)
        {
            newMessageToUI?.Invoke(this, s);
            Console.WriteLine(s);
        }

        internal void StopConnectivity()
        {
            TCPTokenSource.Cancel();
        }
    }
}
