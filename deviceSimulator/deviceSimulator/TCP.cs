using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace deviceSimulator
{
    class TCP
    {
        public event EventHandler<string> receivedTCPMessage;

        private int basePort = 60100;
        private TcpClient client = null;
        NetworkStream stream = null;

        public TCP(string IPString)
        {
            client = new TcpClient("192.168.0.17", basePort);
            stream = client.GetStream();
        }

        public void Send(string msg)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes("writing to TCP");
            stream.Write(data, 0, data.Length);
        }

        public void Close()
        {
            stream.Close();
            client.Close();
        }
    }
}
