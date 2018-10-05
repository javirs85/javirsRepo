using GameBrainControl;
using gameTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GB
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AutoPuzzlesPage : ContentPage
	{
        public AutoPuzzlesPage()
        {
            InitializeComponent();

            PuzzlesContainer.ItemsSource = GameBrain.Puzzles;
            ListSize.Text = GameBrainControl.GameBrain.Puzzles.Count.ToString();
        }

        private void ContentPage_Focused(object sender, FocusEventArgs e)
        {
            
        }
    }
}