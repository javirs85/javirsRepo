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
        }

        public void AddPuzzleContainer()
        {

            Grid g = new Grid() { BackgroundColor = Color.White, Margin = new Thickness(0,10,10,0), ColumnSpacing = 0};
            g.ColumnDefinitions = new ColumnDefinitionCollection();
            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80, GridUnitType.Absolute) });
            g.RowDefinitions = new RowDefinitionCollection();
            g.RowDefinitions.Add(new RowDefinition() { Height = 80 });
            g.RowDefinitions.Add(new RowDefinition() { Height = 80 });

            MainContainer.Children.Add(g);


            

            Button OpenButton = new Button() { Text = "OPEN" };
            g.Children.Add(OpenButton);
            Grid.SetColumn(OpenButton, 1);
            Button resetButton = new Button() { Text = "RESET"};
            g.Children.Add(resetButton);
            Grid.SetColumn(resetButton, 1);
            Grid.SetRow(resetButton, 1);

            Grid inner = new Grid();
            inner.RowSpacing = 0;
            inner.Padding = 10;
            inner.RowDefinitions = new RowDefinitionCollection();
            inner.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20, GridUnitType.Absolute) });
            inner.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            g.Children.Add(inner);
            Grid.SetRowSpan(inner, 2);

            Label tittle = new Label() { TextColor = Color.Black };
            inner.Children.Add(tittle);
            Label status = new Label() { TextColor = Color.Black, HorizontalTextAlignment = TextAlignment.End };
            inner.Children.Add(status);

            Grid yourContent = new Grid();
            Label details = new Label { Text = "details go here", TextColor = Color.Black };
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
