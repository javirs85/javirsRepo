using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using GameBrainControl;

namespace GB
{
    public partial class PuzzlesPage : ContentPage
    {
        public GameBrain Brain;

        public PuzzlesPage()
        {
            InitializeComponent();
            AddPuzzleContainer();
            AddPuzzleContainer();
            AddPuzzleContainer();
        }

        public void AddPuzzleContainer()
        {

            Grid g = new Grid() { BackgroundColor = Color.White, Margin = new Thickness(10), ColumnSpacing = 0 };
            g.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition() { Width = new GridLength(80, GridUnitType.Absolute) }
            };

            MainContainer.Children.Add(g);

            StackLayout buttonsContainer = new StackLayout();
            g.Children.Add(buttonsContainer);
            Grid.SetColumn(buttonsContainer, 1);

            Button OpenButton = new Button() { Text = "OPEN" };
            buttonsContainer.Children.Add(OpenButton);
            Button resetButton = new Button() { Text = "RESET"};
            buttonsContainer.Children.Add(resetButton);
            
            Grid inner = new Grid();
            g.Children.Add(inner);

            Label tittle = new Label();
            inner.Children.Add(tittle);
            Label status = new Label() { HorizontalTextAlignment = TextAlignment.End };
            inner.Children.Add(status);

            Grid yourContent = new Grid();
            Label details = new Label { Text = "details go here", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center};
            yourContent.Children.Add(details);
            inner.Children.Add(yourContent);
            
            /*

            <Grid BackgroundColor="Transparent" Grid.RowSpan="2" RowSpacing="0" Padding="10">
                <Grid.RowDefinitions>
                    <RowDefinition Height="20"/>
                    <RowDefinition Height="*"/>
                </Grid.RowDefinitions>
                <Label Style="{StaticResource BlackLabel}">Relojes</Label>
                <Label Style="{StaticResource BlackLabel}" HorizontalTextAlignment="End"  Grid.Column="1">Sin Tocar</Label>
                <Grid x:Name="ContentHere">
                    
                </Grid>
            </Grid>
        </Grid>
             */
        }

    }
}
