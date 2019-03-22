using System;
using System.Collections.Generic;
using System.Text;
using static Brain.Enums;

namespace Brain
{
    public class CodePuzzle : Puzzle
    {
        public string CurrentValue;

        public CodePuzzle()
        {
            this.Kind = PuzzleKinds.Code;
        }

        public CodePuzzle(string ID)
        {
            this.ID = ID;
            this.Kind = PuzzleKinds.Code;
        }

        public override void SolvedInternal()
        {
            // do nothing
        }

        public override void UpdateSolutionInternal(string newVal)
        {
        }

        public override void UpdateMeasure(string measure)
        {
            CurrentValue = measure;
            requestUIValueUpdate();
        }
    }
}
