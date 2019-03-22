using System;
using System.Collections.Generic;
using System.Text;
using static Brain.Enums;

namespace Brain
{
    public class SimpleSensorPuzzle : Puzzle
    {
        private float _currentValue, _solution;
        public float CurrentValue
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;
                CurrentValueStringyfied = _currentValue.ToString();
            }
        }
        public float Solution
        {
            get { return _solution; }
            set
            {
                _solution = value;
                CurrentSolutionStringyfied = _solution.ToString();
            }
        }

        public SimpleSensorPuzzle()
        {
            this.Kind = PuzzleKinds.Sensor;
        }

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
            
            float.TryParse(newVal, out float sol);
            Solution = sol;
        }

        public override void UpdateMeasure(string measure)
        {
            float.TryParse(measure, out float parsedValue);
            CurrentValue = parsedValue;
            requestUIValueUpdate();
        }
    }
}
