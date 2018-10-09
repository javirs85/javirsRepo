using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using GBCore;


namespace GB
{
    public partial class DebugPage : ContentPage
    {
        public DebugPage()
        {
            InitializeComponent();
            
            GameItems.Brain.newMessageToUI += NewMessageFromBrain;

            test1.Text = "Test";
            test2.Text = "Stop TCP";
        }
        

        private void NewMessageFromBrain(object sender, string e)
        {
            Device.BeginInvokeOnMainThread(() => {
                debugContainer.Children.Add(new Label() { Text = e });
            });

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Message", "No test attached", "Ok");
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            GameItems.Brain.StopConnectivity();
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            GameItems.Puzzles[0]?.Send(
                new gameTools.Message()
                {
                    msgType = gameTools.Utils.MessageTypes.debug,
                    Data = new Dictionary<string, string>() {
                        { "msg", "testing" }
                    }
                });
        }
    }
}
