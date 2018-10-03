using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using GameBrainControl;

namespace GB
{
    public partial class DebugPage : ContentPage
    {
        public GameBrain Brain;

        public DebugPage()
        {
            InitializeComponent();

            Brain = new GameBrain();
            Brain.newMessageToUI += NewMessageFromBrain;
            Brain.Init();

            test1.Text = "Test";
            test2.Text = "Stop TCP";
        }

        public DebugPage(GameBrain _brain)
        {
            InitializeComponent();
            Brain = _brain;
            Brain.newMessageToUI += NewMessageFromBrain;
            Brain.Init();
            int a = 2;
            a++;
        }

        private void NewMessageFromBrain(object sender, string e)
        {
            Device.BeginInvokeOnMainThread(() => {
                debugContainer.Children.Add(new Label() { Text = e });
            });

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Message", "someone touched Test1", "Ok");
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Brain.StopConnectivity();
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {

        }
    }
}
