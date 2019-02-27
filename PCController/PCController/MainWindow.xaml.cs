using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Connectivity;
using Brain;
using OxyPlot;

namespace PCController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int counter = 0;
        GameController Game;

        public MainWindow()
        {
            InitializeComponent();
            Message.SerializationError += Debug;
        }

        private void Discover_Click(object sender, RoutedEventArgs e)
        {
            Game.SendDiscoverMessage();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Game = new GameController();
            Game.Debug += Debug;
            Game.Error += (o, e2) => 
            {
                Debug(this, e2.InnerException?.Message ?? e2.Message);
            };

            var LDR = new SimpleSensorPuzzle("LDR");
            LDR.ValueChanged += LDR_ValueChanged;
            LDR.SatusChanged += LDR_SatusChanged;
            LDR.SolutionChanged += LDR_SolutionChanged;
            Game.AddNewPuzzle(LDR);
        }

        private void LDR_SolutionChanged(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => {
                solutionTB.Text = (sender as Puzzle).CurrentSolutionStringyfied;
            });
        }

        private void LDR_SatusChanged(object sender, EventArgs e)
        {
            var p = sender as Puzzle;
            SolidColorBrush newColor = Brushes.Lavender;
            switch (p.CurrentStatus)
            {
                case Puzzle.AvailableStatus.Online:
                    newColor = Brushes.Wheat;
                    break;
                case Puzzle.AvailableStatus.Solved:
                    newColor = Brushes.LightGreen;
                    break;
                case Puzzle.AvailableStatus.OFFLine:
                    newColor = Brushes.Lavender;
                    break;
                case Puzzle.AvailableStatus.Solving:
                    newColor = Brushes.DarkKhaki;
                    break;
                default:
                    newColor = Brushes.Tomato;
                    break;
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                LDRDetails.Background = newColor;
                StatusTB.Text = p.CurrentStatus.ToString();
            });
        }

        private void LDR_ValueChanged(object sender, EventArgs e)
        {
            float sample = (sender as SimpleSensorPuzzle).CurrentValue;

            Application.Current.Dispatcher.Invoke(() =>
            {
                Data.Add(new DataPoint(counter, sample));
                valueTB.Text = sample.ToString();
            });
            counter++;
        }

        private void Debug(object sender, string e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                DebugPanel.Children.Add(new Label() { Content = e });
                debugScroller.ScrollToBottom();
            });
        }

        private void TestingSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Game.Client.IsConnected)
                Game.Client.PublishRAW(e.NewValue.ToString(), "master/horizontal");
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            Game.StartConnectivity();
        }

        public ObservableCollection<DataPoint> Data
        {
            get { return (ObservableCollection<DataPoint>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ObservableCollection<DataPoint>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<DataPoint>()));

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Data = new ObservableCollection<DataPoint>();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            var game = Game.GetPuzzle("LDR");
            game.RequestReset();
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            var game = Game.GetPuzzle("LDR");
            game.RequestForceSolve();
        }
    }
}
