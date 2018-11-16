using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace gameTools
{
    public class SimpleStringPuzzle : Puzzle
    {
 
        private string _sensedValue = "nothing from device";

        public string SensedValueStr {
            get
            {
                return _sensedValue;
            }

            set
            {
                if(_sensedValue != value)
                {
                    _sensedValue = value;
                    OnPropertyChanged("SensedValueStr");
                }
            }
        }

        protected override void CustomUpdate(Dictionary<string, object> data)
        {
            throw new NotImplementedException();
        }
        
        protected override void Update(object data)
        {
            SensedValueStr = data as string;
        }
    }
}
