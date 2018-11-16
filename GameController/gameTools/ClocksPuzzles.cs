using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace gameTools
{
    public class ClocksPuzzles : Puzzle
    {
        public Label r1, r2, r3,r4, r5, s1, s2, s3, s4, s5;

        public override void GetUIElements(Grid grid)
        {
            r1 = grid.FindByName<Label>("r1");
            r2 = grid.FindByName<Label>("r2");
            r3 = grid.FindByName<Label>("r3");
            r4 = grid.FindByName<Label>("r4");
            r5 = grid.FindByName<Label>("r5");
            s1 = grid.FindByName<Label>("s1");
            s2 = grid.FindByName<Label>("s2");
            s3 = grid.FindByName<Label>("s3");
            s4 = grid.FindByName<Label>("s4");
            s5 = grid.FindByName<Label>("s5");
        }

        public override void UpdateUI()
        {
            ;
            /*if (Details["r1"] == Details["s1"])
                r1.BackgroundColor = s1.BackgroundColor = Color.Red;*/
        }

        protected override void CustomUpdate(Dictionary<string, object> data)
        {
            throw new NotImplementedException();
        }
        

        protected override void Update(object data)
        {
            throw new NotImplementedException();
        }
    }
}
