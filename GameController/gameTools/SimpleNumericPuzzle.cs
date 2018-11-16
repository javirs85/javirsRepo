using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace gameTools
{
    public class SimpleNumericPuzzle : Puzzle
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

        protected override void CustomUpdate(Dictionary<string, object> data)
        {
            throw new NotImplementedException();
        }

        protected override void Update(object data)
        {
            if(!Double.TryParse(data.ToString(), out double parsedValue))
            {
                SensedValue = 0.00;
            }
            else
            {
                SensedValue = parsedValue;
            }
                
        }
    }
}
