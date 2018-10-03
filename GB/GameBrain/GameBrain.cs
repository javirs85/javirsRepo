using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using gameTools;
using Communication;

namespace GameBrainControl
{
    public class GameBrain
    {
        public event EventHandler<string> MajorError;
        public event EventHandler<string> newMessageToUI;
        private ServerController Server;

        public static CancellationTokenSource TCPTokenSource;
        public static CancellationToken TCPcancelToken;

        public static List<Puzzle> puzzles;


        public void Init()
        {
            DebugMessage("initializing gameBrain");

            puzzles = new List<Puzzle>();

            Server = new ServerController();
            Server.newDebugMessage += Debug;
            Server.newDeviceConnected += Server_newDeviceConnected;
            Server.Start();
        }

        private void Server_newDeviceConnected(object sender, System.Net.Sockets.TcpClient client)
        {
            Puzzle puzzle = new Puzzle();
            puzzle.Connect(client);
            puzzle.ID = puzzles.Count;
            puzzle.newDebugMessage += Debug;
            puzzle.newMessageFromPuzzle += ProcessNewMessageFromPuzzle;
            puzzle.PuzzleDisconnected += (o, e) => {
                var p = o as Puzzle;
                if(puzzles.Contains(p)) puzzles.Remove(p);
                Debug($"puzzle with ID: {p.ID} disconnected (connected:{puzzles.Count})");
            };
            //puzzles.Add(puzzle);            

            Debug(null, "TCP Client connected (" + puzzles.Count + ")");

        }

        private void ProcessNewMessageFromPuzzle(object sender, Message e)
        {
            var puzzle = sender as Puzzle;
            if (e.msgType == Utils.MessageTypes.present)
            {
                var newID = int.Parse(e.Data["myID"]);

                if (puzzles.Exists(x => x.ID == newID))
                {
                    Debug($"Tried to set a device with ID: {puzzle.ID}, but it already exists");
                }
                else
                {
                    puzzle.ID = newID;
                    puzzle.Name = e.Data["myName"];
                    Enum.TryParse<Utils.PuzzleStatus>(e.Data["myStatus"], out puzzle.Status);
                    Enum.TryParse<Utils.PuzzleKinds>(e.Data["myKind"], out puzzle.Kind);
                    puzzle.Details = e.Data["myDetails"];

                    puzzles.Add(puzzle);
                    Debug($"Puzzle with ID:{puzzle.ID} succesfully connected");
                }
            }else if(e.msgType == Utils.MessageTypes.update)
            {
                foreach(var field in e.Data)
                {
                    if      (field.Key == "Name")    puzzle.Name = field.Value;
                    else if (field.Key == "Status")  Enum.TryParse<Utils.PuzzleStatus>(field.Value, out puzzle.Status);
                    else if (field.Key == "Kind")    Enum.TryParse<Utils.PuzzleKinds>(field.Value, out puzzle.Kind);
                    else if (field.Key == "Details") puzzle.Details = field.Value;
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
