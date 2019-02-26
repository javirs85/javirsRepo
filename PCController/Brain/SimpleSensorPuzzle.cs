using System;
using System.Collections.Generic;
using System.Text;

namespace Brain
{
    public class SimpleSensorPuzzle : Puzzle
    {

        public float CurrentValue;
        public float Solution;

        public SimpleSensorPuzzle(string ID)
        {
            this.ID = ID;
            this.Kind = PuzzleKinds.Sensor;
        }
        
        public override void SolvedInternal()
        {
            // do nothing
        }

        public override void UpdateSolutionInternal(string newVal)
        {
            float.TryParse(newVal, out Solution);
        }

        public override void UpdateMeasure(string measure)
        {
            float.TryParse(measure, out CurrentValue);
            requestUIValueUpdate();
        }
    }
}
