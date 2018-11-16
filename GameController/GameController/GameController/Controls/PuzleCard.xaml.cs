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

        public Grid ViewContainer
        {
            get { return PuzzleViewer; }
            set { PuzzleViewer = value; }
        }

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
                            var animate = new Animation(d => crossIcon.WidthRequest = d, 40, 270, Easing.CubicIn);
                            animate.Commit(crossIcon, "crossIcon", 16, 250);
                        }
                        else
                        {
                            crossIcon.RotateTo(0, 250, Easing.Linear);
                            ButtonsLayout.TranslateTo(85, 0, 200, Easing.CubicOut);
                            var animate = new Animation(d => crossIcon.WidthRequest = d, 270, 40, Easing.CubicIn);
                            animate.Commit(crossIcon, "crossIcon", 16, 200);                      
                        }
                    })
                }
            );

            /*
            OfflineCover.GestureRecognizers.Add(
                new TapGestureRecognizer()
                {
                    Command = new Command(() => {
                        HideOfflineCover();
                    })
                }
                );
                */
        }

        public async void HideOfflineCover()
        {
            if (OfflineCover.IsVisible)
            {
                PuzzleViewer.IsVisible = true;
                await OfflineCover.FadeTo(0, 250, Easing.CubicIn);
                OfflineCover.IsVisible = false;
            }
        }

        public async void ShowOfflineCover()
        {
            if (OfflineCover.IsVisible == false)
            {
                PuzzleViewer.IsVisible = false;
                OfflineCover.IsVisible = true;
                await OfflineCover.FadeTo(1, 250, Easing.CubicInOut);
            }
        }

        public async void ShowSolvedCover()
        {
            await SolvedCover.FadeTo(1, 250, Easing.CubicIn);
        }

        public async void HideSolvedCover()
        {
            await SolvedCover.FadeTo(0, 250, Easing.CubicInOut);
        }

        public void BindTo(Puzzle p)
        {
            this.BindingContext = p;
            p.StatusChanged += (s1, newStatus) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    switch (newStatus)
                    {
                        case Puzzle.PuzzleStatus.unsolved:
                            HideOfflineCover();
                            HideSolvedCover();
                            break;
                        case Puzzle.PuzzleStatus.solved:
                            HideOfflineCover();
                            ShowSolvedCover();
                            break;
                        case Puzzle.PuzzleStatus.offline:
                            ShowOfflineCover();
                            break;
                        default:
                            break;

                    }
                });
            };
        }
    }

}