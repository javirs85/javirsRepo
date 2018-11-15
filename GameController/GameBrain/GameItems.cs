using gameTools;
using GBCore.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GBCore
{
    public static class GameItems
    {
        public static ObservableCollection<Puzzle> Puzzles { set; get; }
        public static int PuzzlesCount { get { return GameItems.Puzzles.Count; } }
        public static ConnectivityCore Server;
        public static event EventHandler AllPuzzlesLoaded;

        public static event EventHandler<string> newMessageToUI;

        public static void Init()
        {
            LoadAllPuzzles();
            Server = new ConnectivityCore();
            GameItems.Server.g_NewErrorFromDevice += detailedDebug;
            Server.g_newDeviceConnected += Server_newDeviceConnected;
            Server.g_newDebugMessage += Debug;

            BrainConnector.g_NewNetworkError += (sender, err) => {
                var p = GameItems.FindPuzzle((sender as BrainConnector).remoteID);
                Debug($"Network error at {p.Name} : {err.Message}");
            };
        }

        private static void LoadAllPuzzles()
        {
            Puzzles = new ObservableCollection<Puzzle>();
            Puzzles.Add(new Puzzle() { ID = 1, Name = "Mapa", Kind = Puzzle.PuzzleKinds.genericSensor , Status = Puzzle.PuzzleStatus.offline});
            Puzzles.Add(new Puzzle() { ID = 2, Name = "Relojes", Kind = Puzzle.PuzzleKinds.genericSensor, Status = Puzzle.PuzzleStatus.offline });
            Puzzles.Add(new Puzzle() { ID = 3, Name = "Sonar", Kind = Puzzle.PuzzleKinds.genericSensor, Status = Puzzle.PuzzleStatus.offline });

            AllPuzzlesLoaded(null, EventArgs.Empty);
        }

        private static void Server_newDeviceConnected(object sender, ConnectivityCore.ConnectionInfo info)
        {
            var puzzle = GameItems.FindPuzzle(info.ID);
            if (puzzle == null) Debug($"A device with ID {info.ID} tried to connect. But there is no device with such ID in our system");
            else if (puzzle.Name != info.Name) Debug($"A device with ID {info.ID} tried to connect. But its name is {info.Name} whereas our device ID:{info.ID} is named {puzzle.Name}");
            else
            {
                var Zcon = info.Connector;
                puzzle.SetZcon(Zcon);

                Zcon.g_CannotConnect += detailedDebug;
                Zcon.g_NewErrorFromDevice += detailedDebug;
                Zcon.g_WrongToken += detailedDebug;

                puzzle.Status = Puzzle.PuzzleStatus.unsolved;
                Debug($"{puzzle.Name} with ID {puzzle.ID} successfully connected to the system");
            }
        }

        public static Puzzle FindPuzzle(int ID)
        {
            foreach (Puzzle p in Puzzles)
                if (p.ID == ID)
                    return p;
            return null;
        }


        #region debug messages

        private static void Debug(object sender, BrainMessage msg) => detailedDebug(sender, msg);
        private static void Debug(object sender, Exception ex) => DebugMessage(ex.Message);
        private static void Debug(string s) => DebugMessage(s);
        private static void Debug(object sender, string e) => DebugMessage(e);

        public static void detailedDebug(object sender, BrainMessage msg)
        {
            var p = GameItems.FindPuzzle((sender as BrainConnector).remoteID);
            Debug($"Error at {p.Name} : {msg.Params["Message"].ToString()}");
        }
        public static void detailedDebug(object sender, Exception err)
        {
            var p = GameItems.FindPuzzle((sender as BrainConnector).remoteID);
            Debug($"Error at {p.Name} : {err.Message}");
        }

        public static void DebugMessage(string s)
        {
            newMessageToUI?.Invoke(null, s);
            Console.WriteLine(s);
        }

        #endregion

    }
}
