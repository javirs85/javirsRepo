using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Brain
{
    public class PuzzleController
    {
        private string directory = "Puzzles";
        public List<Puzzle> Puzzles;

        public PuzzleController()
        {
            Puzzles = new List<Puzzle>();

            //Puzzles.Add(new SimpleSensorPuzzle("LDR") { Solution = 666, Name = "LightControl" });
            //Puzzles.Add(new CodePuzzle("Piano") { CurrentSolutionStringyfied = "1243", Name = "TouchButtons" });
            //Save();

            Load();
        }

        public Puzzle Find(string id) => Puzzles.Find(x => x.ID == id);
        public void Add(Puzzle p) => Puzzles.Add(p);

        public void Save()
        {
            Directory.CreateDirectory(directory);

            foreach (var p in Puzzles)
                p.Serialize(Path.Combine(directory, p.ID+".xml"));
        }

        public void Load()
        {
            var files = Directory.GetFiles(directory, "*.xml");
            foreach(var file in files)
            {
                var p = Puzzle.Deserialize<Puzzle>(file);
                if (p is SimpleSensorPuzzle) Puzzles.Add(p as SimpleSensorPuzzle);
                else if (p is CodePuzzle) Puzzles.Add(p as CodePuzzle);
                else
                    throw new Exception("Unexpected type of sensor loaded from file");
            }
        }
    }
}
