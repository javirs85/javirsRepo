using System;
using System.Windows.Input;
using System.Collections.Generic;
using Connectivity;

namespace Brain
{
    public abstract class Puzzle 
    {
        public enum PuzzleKinds { Sensor }
        public enum AvailableStatus { Untouched, Online, Solved, OFFLine, Solving}

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

        public abstract void SolvedInternal();

        public string CurrentValueStringyfied;
        public string CurrentSolutionStringyfied;

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

        public abstract void UpdateSolutionInternal(string newVal);

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

        public abstract void UpdateMeasure(string measure);
    }
}
