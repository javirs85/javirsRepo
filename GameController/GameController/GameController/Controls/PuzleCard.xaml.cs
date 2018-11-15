using gameTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameController.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PuzzleCard : ContentView
    {
        private bool AreButtonShown = false;

        public PuzzleCard()
        {
            InitializeComponent();


            ButtonsLayout.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() => {
                        AreButtonShown = !AreButtonShown;
                        if (AreButtonShown)
                        {
                            ButtonsLayout.TranslateTo(0, 0, 250, Easing.CubicIn);
                            crossIcon.RotateTo(45, 250);
                            // crossIcon.ScaleTo(270 / 40, 250, Easing.CubicIn);
                            var animate = new Animation(d => crossIcon.WidthRequest = d, 40, 270, Easing.CubicIn);
                            animate.Commit(crossIcon, "crossIcon", 16, 250);
                        }
                        else
                        {
                            crossIcon.RotateTo(0, 250, Easing.Linear);
                            ButtonsLayout.TranslateTo(85, 0, 200, Easing.CubicOut);
                            var animate = new Animation(d => crossIcon.WidthRequest = d, 270, 40, Easing.CubicIn);
                            animate.Commit(crossIcon, "crossIcon", 16, 200);
                            // await crossIcon.ScaleTo(1, 100, Easing.CubicIn);
                            
                        }
                    })
                }
            );

            OfflineCover.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() => {
                        HideOfflineCover();
                    })
                }
                );
        }

        public async void HideOfflineCover()
        {
            await OfflineCover.FadeTo(0, 250, Easing.CubicIn);
            OfflineCover.IsVisible = false;
        }

        public async void ShowSolvedCover()
        {
            await SolvedCover.FadeTo(1, 250, Easing.CubicIn);
        }

        public void BindTo(Puzzle p)
        {
            this.BindingContext = p;
        }
    }

}