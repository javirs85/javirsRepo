using System;
using System.Collections.Generic;
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

namespace MIResultSetEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string SourcePath = "";
        private Zeus.ResultSet result;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                OpenFile(files[0]); 
            }
        }

        private void OpenFile(string v)
        {
            result = Zeus.ResultSet.LoadFromBin<Zeus.ResultSet>(v);
            yearB.Text = result.date.Year.ToString();
            MonthB.Text = result.date.Month.ToString();
            DayB.Text = result.date.Day.ToString();
            AccuracyBlock.Text = result.maxAccuracy.ToString();
                        
            SourcePath = System.IO.Path.GetDirectoryName(v);
            
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dirName = $"{yearB.Text}{MonthB.Text}{DayB.Text}_152200";
            var folder = System.IO.Path.Combine(SourcePath, dirName);
            System.IO.Directory.CreateDirectory(folder);


            try
            {
                result.date = new DateTime(int.Parse(yearB.Text), int.Parse(MonthB.Text), int.Parse(DayB.Text), 15, 22, 00);
                result.maxAccuracy = int.Parse(AccuracyBlock.Text);
                result.saveToBin(folder + @"/results.bin");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
