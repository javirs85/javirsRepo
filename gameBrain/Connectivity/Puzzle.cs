using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameSystem
{
    public class Puzzle
    {
        public int ID;
        public string Name;
        public Utils.PuzzleStatus Status;
        public string Details;
        public Utils.PuzzleKinds kind;
        public string IP;
        public bool IsOnline = false;

        public TCPController TCP;

        public Puzzle(string _ip, Utils.PuzzleKinds _kind)
        {
            this.IP = _ip;
            this.kind = _kind;

            this.TCP = new TCPController();
            TCP.NewTCPMessage += TCP_NewTCPMessage;
            this.TCP.Connect(this.IP);
        }

        private void TCP_NewTCPMessage(object sender, string e)
        {
            gameBrain.Debug(e);
        }

        public void Reset()
        {
            SendMsg(Utils.MessageTypes.reset);
        }
        public void SolveForced() { }

        private void SendMsg(Utils.MessageTypes _msgType, Dictionary<string, string> _data = null)
        {
            if (IP == null)
                throw new Exception("set IP first");

            Message m = new Message
            {
                data = _data,
                msgType = _msgType
            };

            string msg = m.Serialize();
            UDPController.Send(msg, this.IP, false);
        }
    }
}
