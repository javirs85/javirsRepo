using Communication;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace gameTools
{
    public class Puzzle : INotifyPropertyChanged
    {
        public event EventHandler<string> newDebugMessage;
        public event EventHandler<Message> newMessageFromPuzzle;
        public event EventHandler PuzzleDisconnected;

        public event EventHandler StatusChanged;
        public event EventHandler DetailsChanged;
        public event EventHandler PuzzleSolved;
        public event PropertyChangedEventHandler PropertyChanged;

        public int ID;
        private string _name;
        public string Name {
            get { return _name; }
            set {
                if (_name != value) _name = value;
                OnPropertyChanged("Name");
            }
        }
        private Utils.PuzzleStatus _status;
        public Utils.PuzzleStatus Status {
            get { return _status; }
            set {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        private string _details;
        public string Details {
            get { return _details; }
            set {
                if (_details != value)
                {
                    _details = value;
                    OnPropertyChanged("Details");
                }
            }
        }
        public Utils.PuzzleKinds Kind { get; set; }
        public string IP;
        public bool IsOnline = false;

        public TCPController TCP;

        public System.Windows.Input.ICommand ForceOpen { get; private set; }
        public System.Windows.Input.ICommand ForceReset { get; private set; }

        public Puzzle()
        {
            ForceOpen = new Command(() => {
                Debug($"{Name} clicked OPEN");

                var m = new Message() { msgType = Utils.MessageTypes.forceSolve };
                TCP.Send(m.Serialize());
            });
            ForceReset = new Command(() => {
                Debug($"{Name} clicked RESET");
                var m = new Message() { msgType = Utils.MessageTypes.reset };
                TCP.Send(m.Serialize());
            });
        }
        
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

        private void Debug(string s) => Debug(null, s);

        private void Debug(object sender, string e)
        {
            newDebugMessage?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged (string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}