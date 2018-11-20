using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GameController.Controls;
using gameTools;

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
                    var Card = new PuzzleCard();
                    Card.BindTo(p);


                    if (p is SimpleNumericPuzzle)
                    {
                        var viewer = new DefaultViewer();
                        Card.ViewContainer.Children.Add(viewer);
                    }
                    else if (p is MapPuzzle)
                    {
                        var viewer = new MapViewer();
                        viewer.setPuzzle(p);
                        Card.ViewContainer.Children.Add(viewer);
                    }
                    else if (p is SimpleStringPuzzle)
                    {
                        var viewer = new DefaultViewer();
                        Card.ViewContainer.Children.Add(viewer);
                    }
                    
                    puzlesContainer.Children.Add(Card);
                    Card.ShowOfflineCover();
                }
            };
        }
	}
}