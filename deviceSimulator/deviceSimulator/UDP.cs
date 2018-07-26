using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace deviceSimulator
{
    class UDP
    {
        public event EventHandler<string> receivedTCPMessage;

        private int basePort = 60100;
        private UdpClient client = null;
        IPEndPoint groupEP = null;

        public UDP(string IPString)
        {
            client = new UdpClient();
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse("255.255.255.255"), basePort);
        }

        public void Send(string msg)
        {
            byte[] sendBytes4 = Encoding.ASCII.GetBytes(msg);
            client.Send(sendBytes4, sendBytes4.Length, groupEP);
        }

        public void Close()
        {
        }
    }
}
