using System;
using System.Windows.Input;
using System.Collections.Generic;
using Connectivity;
using System.IO;
using System.Xml.Serialization;
using static Brain.Enums;

namespace Brain
{

    [Serializable]
    [XmlInclude(typeof(SimpleSensorPuzzle))]
    [XmlInclude(typeof(CodePuzzle))]
    public class Puzzle
    {
        public string CurrentValueStringyfied;
        public string CurrentSolutionStringyfied;


        private Connectivity.Client Connection;

        public PuzzleKinds Kind { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        private string publishChannel { get { return "master/" + ID; } }
        private AvailableStatus _curretStatus;
        public AvailableStatus CurrentStatus
        {
            get { return _curretStatus; }
            set
            {
                _curretStatus = value;
                requestUIStatusUpdate();
            }
        }

        public void RequestForceSolve()
        {
            Connection?.Publish(new Message(Message.AvailableOrders.forceSolve), publishChannel);
        }
        public void RequestReset()
        {
            Connection?.Publish(new Message(Message.AvailableOrders.Reset), publishChannel);
        }
        public void RequestUpdate()
        {
            Connection.Publish(new Message(Message.AvailableOrders.UpdateRequested), publishChannel);
        }
        public void SetNewSolution(string newSolution)
        {
            Connection.Publish(
                new Message(
                    Message.AvailableOrders.setThisNewSolution)
                        {
                            Params = new Dictionary<string, string>() { { "newSolution", newSolution } }
                        },
                    publishChannel);
        }
        public void SetYourCurrentValueAsSolution()
        {
            Connection?.Publish(
                new Message(
                    Message.AvailableOrders.UpdateYOURSolution),
                    publishChannel);
        }
        public void SetCurrentAsSolution()
        {
            Connection.Publish(
                new Message(
                    Message.AvailableOrders.UpdateYOURSolution)
                {
                    Params = new Dictionary<string, string>() { { "newSolution", "currentValues" } }
                },
                    publishChannel);
        }
        public void Solved()
        {
            CurrentStatus = AvailableStatus.Solved;
            SatusChanged?.Invoke(this, EventArgs.Empty);
        }



        public event EventHandler SatusChanged;
        public event EventHandler ValueChanged;
        public event EventHandler SolutionChanged;

        public Puzzle()
        {
            CurrentStatus = AvailableStatus.OFFLine;
        }

        public  void UpdateSolution(string newVal)
        {
            CurrentSolutionStringyfied = newVal;
            UpdateSolutionInternal(newVal);
            SolutionChanged?.Invoke(this, EventArgs.Empty);
        }


        public void SetConnector(Client client)
        {
            this.Connection = client;
        }

        protected void requestUIValueUpdate() => ValueChanged?.Invoke(this, EventArgs.Empty);
        protected void requestUIStatusUpdate() => SatusChanged?.Invoke(this, EventArgs.Empty);


        public void Init(Connectivity.Client con, PuzzleKinds kind)
        {
            Connection = con;
            Kind = kind;
        }


        public void Serialize(string path)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(/*this.GetType()*/typeof(Puzzle));
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.IO.Path.GetFullPath(path)))
                {
                    xml.Serialize(file, this);
                }
            }
            catch (Exception e)
            {
                ;
            }
        }

        public static T Deserialize<T>(string path)
        {
            T loaded = default(T);
            try
            {
                System.Xml.Serialization.XmlSerializer deserializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                using (StreamReader reader = new StreamReader(path))
                {
                    loaded = (T)deserializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                ;
            }
            return loaded;
        }

        public virtual void SolvedInternal()
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateSolutionInternal(string newVal)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateMeasure(string measure)
        {
            throw new NotImplementedException();
        }
    }
}
