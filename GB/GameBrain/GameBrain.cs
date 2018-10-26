using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using gameTools;
using Communication;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace GBCore
{
    public class GameBrain
    {
        public event EventHandler<string> MajorError;
        public event EventHandler<string> newMessageToUI;
        public event EventHandler<Puzzle> newPuzzleAdded;

        public static CancellationTokenSource TCPTokenSource;
        public static CancellationToken TCPcancelToken;

        private List<Puzzle> Limbo = new List<Puzzle>();
        

        public void Init()
        {
            DebugMessage("initializing gameBrain");

            GameItems.Server.newDebugMessage += Debug;
            GameItems.Server.newDeviceConnected += Server_newDeviceConnected;
        }

        public void AddPuzzle(Puzzle puzzle)
        {
            GameItems.Puzzles.Add(puzzle);
            newPuzzleAdded?.Invoke(this, puzzle);
        }


        private void Server_newDeviceConnected(object sender, System.Net.Sockets.TcpClient client)
        {
            Puzzle puzzle = new Puzzle();
            puzzle.Connect(client);
            puzzle.newDebugMessage += Debug;
            puzzle.newMessageFromPuzzle += ProcessNewMessageFromPuzzle;
            puzzle.PuzzleDisconnected += (o, e) => {
                var p = o as Puzzle;
                if (GameItems.Puzzles.Contains(p)) GameItems.Puzzles.Remove(p);
                Debug($"puzzle with ID: {p.ID} disconnected (connected now:{GameItems.Puzzles.Count})");
            };

            Limbo.Add(puzzle);


            Debug(null, "TCP Client connected (" + GameItems.Puzzles.Count + ")");

        }

        private void ProcessNewMessageFromPuzzle(object sender, Message e)
        {
            var puzzle = sender as Puzzle;


            if (e.msgType == Utils.MessageTypes.present)
            {
                var newID = int.Parse(e.Data["myID"]);

                if (GameItems.Puzzles.Any(x => x.ID == newID))
                {
                    Debug($"Tried to set a device with ID: {puzzle.ID}, but it already exists");
                }
                else
                {
                    if (Limbo.Contains(puzzle))
                    {
                        Utils.PuzzleKinds tempKind;
                        if (Enum.TryParse<Utils.PuzzleKinds>(e.Data["myKind"], out tempKind))
                            puzzle.Kind = tempKind;
                        else
                            throw new Exception($"unexepcted puzzle kind {e.Data["myKind"]}");
                        
                        if (tempKind == Utils.PuzzleKinds.Clocks)
                            puzzle = new ClocksPuzzles();
                        else
                            puzzle = new Puzzle();

                        puzzle.Kind = tempKind;


                        puzzle.ID = newID;
                        puzzle.Name = e.Data["myName"];
                        puzzle.Details = e.Details;
                        Utils.PuzzleStatus tempStatus;
                        if (Enum.TryParse<Utils.PuzzleStatus>(e.Data["myStatus"], out tempStatus))
                            puzzle.Status = tempStatus;
                        else
                            throw new Exception($"Unexpected status {e.Data["myStatus"]}");                        

                        Debug($"Puzzle with ID:{puzzle.ID} succesfully connected");

                        AddPuzzle(puzzle);
                        Limbo.Remove(puzzle);
                    }
                    else
                    {
                        Debug($"Received a present from {puzzle.Name}({puzzle.ID}) that was not in the Limbo so it won't be added");
                    }
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
                    else
                        MajorError?.Invoke(this, $"Unexpected field {field.Key} when updating a puzzle");
                }
                if (e.Details != puzzle.Details)
                {
                    puzzle.Details = e.Details;

                    puzzle.UpdateUI();
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
