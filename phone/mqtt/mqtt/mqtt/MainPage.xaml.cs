using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Mqtt;
using System.Net;

namespace mqtt
{
    public partial class MainPage : ContentPage
    {
        // old system.Net.mqtt IMqttClient client;

        Connectivity.Client Client;

        string lastV,lastH = "";


        public MainPage()
        {
            try
            {
                InitializeComponent();
                Client = new Connectivity.Client();
                Client.Error += (o, e) => Debug(e.InnerException.Message);
                Client.Debug += Debug;
                Client.NewMessage += Debug;

                var host = Dns.GetHostEntry(Dns.GetHostName());
                List<IPAddress> ips = new List<IPAddress>();
                foreach(var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        ips.Add(ip);
                }

                Debug("creating ... ");
                Client.Start();

                Client.Subscrive("devices/#");
                Client.Subscrive("$SYS/#");
            }
            catch (Exception e)
            {
                ;
            }
        }

        #region debug messages
        public void Debug(object o, string str) => Debug(str);
        public void Debug(string str)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
                mainStack.Children.Add(new Label() { Text = str });
                // await scrollerMain.ScrollToAsync(mainStack, ScrollToPosition.End, true);
            });
        }
        #endregion

        private void SendMsg_Clicked(object sender, EventArgs e)
        {
            Client.Publish("SHOWUP");
        }

        private void CreateClient_Clicked(object sender, EventArgs e)
        {
            Client.Publish("debug message");
        }

        private void Subscribe_Clicked(object sender, EventArgs e)
        {
            Debug("already subscrived to #");
        }

        private void VerticalSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / 1);
            verticalSlider.Value = newStep;
            verticalSliderLabel.Text = verticalSlider.Value.ToString();
            var toSend = newStep.ToString();
            if (toSend != lastV)
            {
                lastV = toSend;
                Client?.Publish(lastV, "master/vertical");
            }
        }

        private void HorizontalSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / 1);
            horizontalSlider.Value = newStep ;
            horizontalSliderLabel.Text = horizontalSlider.Value.ToString();
            var toSend = newStep.ToString();
            if (toSend != lastH)
            {
                lastH = toSend;
                Client?.Publish(toSend, "master/vertical");
            }
        }
    }

    /*
    
    old:

    
            /*
               var server = MqttServer.Create();
               server.ClientConnected += Debug;
               server.ClientDisconnected += Debug;
               server.Stopped += (o, e) => Debug($"disconnection ! {e.Message} and {e.Reason} ");

               server.Start();
               Debug("server started");*/


     /* //old system.Net.mqtt thing

     var configuration = new MqttConfiguration();
     this.client = await MqttClient.CreateAsync("192.168.110.51", configuration);

     client.Disconnected += (o, e) => Debug($"disconnection (at client level)! {e.Message} and {e.Reason} ");
     var state = await client.ConnectAsync(new MqttClientCredentials(clientId: "tester"));

     Debug("Client connected to broker!");

     await client.SubscribeAsync(topic, MqttQualityOfService.AtLeastOnce);
     Debug($"Client subscrived to {topic}");
     */


     /* // old system.Net.mqtt thing

     var configuration = new MqttConfiguration();
     var msg = new MqttApplicationMessage(topic, Encoding.UTF8.GetBytes("msg from xamarin"));
     await client.PublishAsync(msg, MqttQualityOfService.AtLeastOnce);
     */


     /* // old system.Net.mqtt thing
    client
                .MessageStream
                .Subscribe(msg => Debug($"Message received in topic"));

     client.MessageStream.Subscribe(msg => {
         MessageSub = System.Text.Encoding.UTF8.GetString(msg.Payload);
         Debug(MessageSub);
     });
     */
     
}
