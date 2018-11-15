using GBCore.Connectivity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace gameTools
{
    public class Puzzle : INotifyPropertyChanged
    {
        public enum PuzzleKinds { genericSensor, motor, temperature, button, server, Clocks }
        public enum PuzzleStatus { unsolved, solved, offline };

        public event EventHandler<string> newDebugMessage;

        public event EventHandler<Puzzle.PuzzleStatus> StatusChanged;
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
        private Puzzle.PuzzleStatus _status;
        public Puzzle.PuzzleStatus Status {
            get { return _status; }
            set {
                if (_status != value)
                {
                    _status = value;
                    StatusChanged?.Invoke(this, value);
                    OnPropertyChanged("Status");
                }
            }
        }

        private Dictionary<string, string> _details;
        public Dictionary<string, string> Details {
            get { return _details; }
            set
            {
                _details = value;
                OnPropertyChanged("Details");
            }
        }
        public Puzzle.PuzzleKinds Kind { get; set; }
        public string IP;
        public bool IsOnline = false;

        private BrainConnector ZCon;

        
        public System.Windows.Input.ICommand ForceOpen { get; set; }
        public System.Windows.Input.ICommand ForceReset { get; set; }

        public Puzzle()
        {
            ForceOpen = new Command(() => {
                Debug($"{Name} clicked OPEN");

                var m = new BrainMessage() { Order = messageKinds.forceSolve };
                if (ZCon != null)
                    ZCon.Send(m.Serialize());
                else
                    Debug(null, "TCP Client is disconnected)");

            });
            ForceReset = new Command(() => {
                Debug($"{Name} clicked RESET");
                var m = new BrainMessage() { Order = messageKinds.reset };
                ZCon.Send(m.Serialize());
            });
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Puzzle FromString(string s)
        {
            return JsonConvert.DeserializeObject<Puzzle>(s);
        }

        public void SetZcon(BrainConnector conn)
        {
            this.ZCon = conn;
            ZCon.g_DeviceClosed += (s1, a1) => { Status = PuzzleStatus.offline; };
        }

        public void Send(BrainMessage m)
        {
            ZCon.Send(m.Serialize());
        }

        public static Puzzle generateDummy()
        {
            var p = new Puzzle()
            {
                Name = "TestPuzle",
                ID = 22,
                Status = Puzzle.PuzzleStatus.offline,
                Kind = Puzzle.PuzzleKinds.genericSensor
            };
            p.Details = new Dictionary<string, string>();
            p.Details.Add("r1", "10:00");
            p.Details.Add("s1", "15:15");
            p.Details.Add("r2", "20:00");
            p.Details.Add("s2", "16:16");

            return p;
        }

        public virtual void GetUIElements(Grid grid)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateUI()
        {
            throw new NotImplementedException();
        }
             

        private void Debug(string e) => Debug(null, e);
        private void Debug(BrainMessage s) => Debug(null, "Error in device Puzzle ln. 128");
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