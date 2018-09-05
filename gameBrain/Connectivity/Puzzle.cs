using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameBrain
{
    public class Puzzle
    {
        Utils.PuzzleKinds kind;
        string IP;

        public Puzzle(string _ip, Utils.PuzzleKinds _kind)
        {
            this.IP = _ip;
            this.kind = _kind;
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
