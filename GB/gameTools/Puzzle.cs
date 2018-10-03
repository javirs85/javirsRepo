using Communication;
using System;

namespace gameTools
{
    public class Puzzle
    {
        public event EventHandler<string> newDebugMessage;
        public event EventHandler<Message> newMessageFromPuzzle;
        public event EventHandler PuzzleDisconnected;

        public int ID;
        public string Name;
        public Utils.PuzzleStatus Status;
        public string Details;
        public Utils.PuzzleKinds Kind;
        public string IP;
        public bool IsOnline = false;

        public TCPController TCP;
        
        public void Connect(System.Net.Sockets.TcpClient client)
        {
            TCP = new TCPController();
            TCP.newDebugMessage += Debug;
            TCP.newMessageFromServer += preprocessTCPMessage;
            TCP.clientDisconnected += (o, e) => { PuzzleDisconnected?.Invoke(this, EventArgs.Empty); };
            TCP.ListenToClient(client);
        }

        public void Send(Message m)
        {
            TCP.Send(m.Serialize());
        }

        private void preprocessTCPMessage(object sender, string e)
        {
            Message m = Message.Deserialize(e);
            newMessageFromPuzzle?.Invoke(this, m);
        }

        private void Debug(object sender, string e)
        {
            newDebugMessage?.Invoke(this, e);
        }
    }
}