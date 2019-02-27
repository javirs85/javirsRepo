using System;
using System.Collections.Generic;
using System.Text;
using Connectivity;

namespace Brain
{
    public class GameController
    {
        public Client Client;
        public event EventHandler<Exception> Error;
        public event EventHandler<string> Debug;

        private List<Puzzle> Puzzles;

        public GameController()
        {
            Puzzles = new List<Puzzle>();
        }

        public void StartConnectivity()
        {
            Client = new Client();

            Client.Debug += Debug;
            Client.Error += (o, x) => Debug(o, x.InnerException?.Message ?? x.Message);

            Client.newMeasure += Client_newMeasure;
            Client.NewMessage += Client_NewMessage;
            Client.Error += Error;
            Client.ConnectedSucessfully += (o, e) => {
                foreach (var p in Puzzles)
                    p.SetConnector(Client);
            };

            Client.Start();
        }

        private void Client_NewMessage(object sender, Tuple<string, Message> e)
        {
            Debug?.Invoke(this, "entering: "+e.Item2.Order.ToString());
            var puzzle = Puzzles.Find(x => x.ID == e.Item1);
            switch (e.Item2.Order)
            {
                case Message.AvailableOrders.present:
                    if (puzzle.CurrentStatus == Puzzle.AvailableStatus.OFFLine)
                        puzzle.CurrentStatus = Puzzle.AvailableStatus.Online;
                    break;
                case Message.AvailableOrders.ImSolved:
                    puzzle.Solved();
                    break;
                case Message.AvailableOrders.thisIsMySolution:
                    puzzle.UpdateSolution(e.Item2.Params["mySolution"]);
                    break;
                case Message.AvailableOrders.statusUpdate:
                    puzzle.CurrentStatus = (Puzzle.AvailableStatus)Enum.Parse( typeof(Puzzle.AvailableStatus), e.Item2.Params["myStatus"]);
                    break;
                default:
                    Debug(this, $"Unexpected message {e.Item2.Order} received from the puzzle {e.Item1}");
                    break;
            }
        }

        public Puzzle GetPuzzle(string ID) => Puzzles.Find(x => x.ID == ID);

        private void Client_newMeasure(object sender, Tuple<string, string> e)
        {
            var puzzle = Puzzles.Find(x => x.ID == e.Item1);
            puzzle.UpdateMeasure(e.Item2);
        }

        public void SendDiscoverMessage()
        {
            foreach (var p in Puzzles) p.CurrentStatus = Puzzle.AvailableStatus.OFFLine;
            Message m = new Message(Message.AvailableOrders.showup);
            Client.Publish(m);
        }

        public void AddNewPuzzle(Puzzle p)
        {
            p.SetConnector(this.Client);
            Puzzles.Add(p);
        }
    }
}
