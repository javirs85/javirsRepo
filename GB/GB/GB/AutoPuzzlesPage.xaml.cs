using gameTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GBCore;
using System.Reflection;

namespace GB
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AutoPuzzlesPage : ContentPage
	{
        public AutoPuzzlesPage()
        {
            InitializeComponent();
            //var a = this.PuzzlesContainer;

            GameItems.Brain.newPuzzleAdded += (o, puzzle) => {
                if (puzzle.Kind == Utils.PuzzleKinds.Clocks)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        GB.Controls.PuzzleViewer viewer = new Controls.PuzzleViewer
                        {
                            BindingContext = puzzle
                        };
                        PuzzlesStack.Children.Add(viewer);
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        GB.Controls.SimpleSensorViewer viewer = new Controls.SimpleSensorViewer
                        {
                            BindingContext = puzzle as SimpleSensorPuzzle
                        };
                        PuzzlesStack.Children.Add(viewer);
                    });
                }
                //old : ConnectUIWithPuzzles();
            };
        }
        /*
        public void ConnectUIWithPuzzles()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IEnumerable<PropertyInfo> pInfos = (PuzzlesContainer as ItemsView<Cell>).GetType().GetRuntimeProperties();
                var templatedItems = pInfos.FirstOrDefault(info => info.Name == "TemplatedItems");
                if (templatedItems != null)
                {
                    var cells = templatedItems.GetValue(PuzzlesContainer);
                    foreach (ViewCell cell in cells as Xamarin.Forms.ITemplatedItemsList<Xamarin.Forms.Cell>)
                    {
                        if (cell.BindingContext != null && cell.BindingContext is Puzzle)
                        {
                            var target = (cell.View as Grid).Children.ToList()[2];
                            if (target != null)
                            {
                                (cell.BindingContext as Puzzle).GetUIElements(target as Grid);
                            }
                        }
                    }
                }
            });
        }
        */
    }

    public class PuzzleDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MapTemplate { get; set; }
        public DataTemplate ClockTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var p = item as Puzzle;
            if (p.Kind == Utils.PuzzleKinds.Clocks) return ClockTemplate;
            else return MapTemplate;            
        }
    }

    public class FirstClockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            return (value as Dictionary<string,string>)[parameter as string];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}