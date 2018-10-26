using Communication;
using gameTools;
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
        public static GameBrain Brain;
        public static ServerController Server;

        public static void Init()
        {
            Brain = new GameBrain();
            Brain.Init();

            Puzzles = new ObservableCollection<Puzzle>();

        }

        public static void StartServer()
        {
            Server = new ServerController();
            Server.Start();
        }
    }
}
