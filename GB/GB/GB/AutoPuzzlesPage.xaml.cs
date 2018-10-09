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

namespace GB
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AutoPuzzlesPage : ContentPage
	{
        public AutoPuzzlesPage()
        {
            InitializeComponent();
        }
    }

    public class PuzzleDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MapTemplate { get; set; }
        public DataTemplate ClockTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var p = item as Puzzle;
            if (p.Name == "Relojes") return ClockTemplate;
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

    public class SecondClockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (value as Dictionary<string, string>)["s1"];
            }
            catch
            {
                return "Json bad format";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}