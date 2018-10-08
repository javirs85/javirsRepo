using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using gameTools;
using Communication;
using System.Collections.ObjectModel;

namespace GameBrainControl
{
    public static class GameItems
    {
        public static ObservableCollection<Puzzle> Puzzles { set; get; }
        public static int PuzzlesCount { get { return GameItems.Puzzles.Count; } }
        public static GameBrain Brain;
        public static ServerController Server;

        public static void Init()
        {
            Brain = new GameBrain();
            Brain.Init();

            Puzzles = new ObservableCollection<Puzzle>();
            /*
            Brain.AddPuzzle(new Puzzle() { Name = "Sonar", Status = Utils.PuzzleStatus.unsolved, Details = "Test sonar puzzle" });
            Brain.AddPuzzle(new Puzzle() { Name = "Mapa", Status = Utils.PuzzleStatus.solved, Details = "magned based map" });
            Brain.AddPuzzle(new Puzzle() { Name = "Relojes", Status = Utils.PuzzleStatus.solved, Details = "15:00 [17:00]" });
            */
        }

        public static void StartServer()
        {
            Server = new ServerController();
            Server.Start();
        }
    }

    public class GameBrain
    {
        public event EventHandler<string> MajorError;
        public event EventHandler<string> newMessageToUI;

        public static CancellationTokenSource TCPTokenSource;
        public static CancellationToken TCPcancelToken;
        


        public void Init()
        {
            DebugMessage("initializing gameBrain");
                       
            GameItems.Server.newDebugMessage += Debug;
            GameItems.Server.newDeviceConnected += Server_newDeviceConnected;
        }

        public void AddPuzzle(Puzzle puzzle)
        {
            GameItems.Puzzles.Add(puzzle);
            puzzle.newDebugMessage += Debug;
            puzzle.newMessageFromPuzzle += ProcessNewMessageFromPuzzle;
            puzzle.PuzzleDisconnected += (o, e) => {
                var p = o as Puzzle;
                if (GameItems.Puzzles.Contains(p)) GameItems.Puzzles.Remove(p);
                Debug($"puzzle with ID: {p.ID} disconnected (connected now:{GameItems.Puzzles.Count})");
            };
        }


        private void Server_newDeviceConnected(object sender, System.Net.Sockets.TcpClient client)
        {
            Puzzle puzzle = new Puzzle();
            puzzle.Connect(client);
            AddPuzzle(puzzle);

            Debug(null, "TCP Client connected (" + GameItems.Puzzles.Count + ")");

        }

        private void ProcessNewMessageFromPuzzle(object sender, Message e)
        {
            var puzzle = sender as Puzzle;
            if (e.msgType == Utils.MessageTypes.present)
            {
                var newID = int.Parse(e.Data["myID"]);

                if (GameItems.Puzzles.AsQueryable().FirstOrDefault(x => x.ID == newID) != null)
                {
                    Debug($"Tried to set a device with ID: {puzzle.ID}, but it already exists");
                }
                else
                {
                    puzzle.ID = newID;
                    puzzle.Name = e.Data["myName"];
                    Utils.PuzzleStatus tempStatus;
                    if (Enum.TryParse<Utils.PuzzleStatus>(e.Data["myStatus"], out tempStatus))
                        puzzle.Status = tempStatus;
                    else
                        throw new Exception($"Unexpected status {e.Data["myStatus"]}");
                    
                    Utils.PuzzleKinds tempKind;
                    if (Enum.TryParse<Utils.PuzzleKinds>(e.Data["myKind"], out tempKind))
                        puzzle.Kind = tempKind;
                    else
                        throw new Exception($"unexepcted puzzle kind {e.Data["myKind"]}");

                    Debug($"Puzzle with ID:{puzzle.ID} succesfully connected");
                }
            }
            else if(e.msgType == Utils.MessageTypes.update)
            {
                foreach(var field in e.Data)
                {
                    if (field.Key == "myName") puzzle.Name = field.Value;
                    else if (field.Key == "myStatus")
                    {
                        Utils.PuzzleStatus st;
                        if (Enum.TryParse<Utils.PuzzleStatus>(field.Value, out st))
                            puzzle.Status = st;
                        else
                            throw new Exception($"Unexpected status {field.Value}");
                    }
                    else if (field.Key == "myKind")
                    {
                        Utils.PuzzleKinds tempKind;
                        if (Enum.TryParse<Utils.PuzzleKinds>(field.Value, out tempKind))
                            puzzle.Kind = tempKind;
                        else
                            throw new Exception($"Unexpected kind {field.Value}");

                    }
                    else if (field.Key == "myDetails") puzzle.Details = field.Value;
                    else
                        MajorError?.Invoke(this, $"Unexpected field {field.Key} when updating a puzzle");
                }
            }
            else
                Debug((sender as Puzzle).ID + " : send a Message");
        }

        private void Debug(string s) => Debug(null, s);

        private void Debug(object sender, string e)
        {
            DebugMessage(e);
        }

        public void DebugMessage(string s)
        {
            newMessageToUI?.Invoke(this, s);
            Console.WriteLine(s);
        }

        public void StopConnectivity()
        {
            TCPTokenSource.Cancel();
        }
    }
}
