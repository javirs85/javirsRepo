using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GameController.Controls;
     

namespace GameController
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PuzzlesPage : ContentPage
	{
		public PuzzlesPage ()
		{
			InitializeComponent ();

            GBCore.GameItems.AllPuzzlesLoaded += (s, e) =>
            {
                foreach(var p in GBCore.GameItems.Puzzles)
                {
                    var ui = new GameController.Controls.PuzleCard();
                    //ui.BindTo(p);
                    var l = new Label() { Text = p.Name };
                    puzlesContainer.Children.Add(l);
                }
            };
        }
	}
}