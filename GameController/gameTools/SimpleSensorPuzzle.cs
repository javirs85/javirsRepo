using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace gameTools
{
    public class SimpleSensorPuzzle : Puzzle
    {
        private double _sensedValue = 2.56;
        

        public double SensedValue
{
            get { return _sensedValue; }
            set
            {
                if (_sensedValue != value) _sensedValue = value;
                OnPropertyChanged("SensedValue");
                OnPropertyChanged("SensedValueStr");
            }
        }

        public string SensedValueStr {get { return SensedValue.ToString();} }
    }
}
