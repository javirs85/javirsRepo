using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connectivity
{
    public class Client
    {
        IManagedMqttClient mqttClient;

        public event EventHandler<Exception> Error;
        public event EventHandler<string> Debug;
        public event EventHandler<Tuple<string, Message>> NewMessage;
        public event EventHandler<Tuple<string, string>> newMeasure;
        public event EventHandler ConnectedSucessfully;

        public bool IsConnected => mqttClient.IsConnected;

        //public static string ServerIP = "localhost";
        public static string ServerIP = "192.168.43.158";

        private DateTime lastMessageTime;

        public Client()
        {
            mqttClient = new MqttFactory().CreateManagedMqttClient();

            mqttClient.SynchronizingSubscriptionsFailed += (o, e) => Error?.Invoke(this, e.Exception);
            mqttClient.ConnectingFailed += (o, e) =>
                {
                    Debug(this, "NOOOO !!");
                    //Error?.Invoke(this, e.Exception);
                };
            mqttClient.Disconnected += (o, e) => Error?.Invoke(this, e.Exception);
            mqttClient.SynchronizingSubscriptionsFailed += (o, e) => Error?.Invoke(this, e.Exception);
            mqttClient.Connected += (o, e) =>
            {
                ConnectedSucessfully?.Invoke(this, EventArgs.Empty);
                Subscrive("#");
                Debug?.Invoke(this, "Client conencted to server");
                Message m = new Message(Message.AvailableOrders.showup);
                Publish(m.Serialize());
            };
            //mqttClient.ApplicationMessageProcessed += (o, e) => Debug?.Invoke(this, "applicationMEssageProcessed: "+e.Exception.Message);
            mqttClient.ApplicationMessageSkipped += (o, e) => Debug?.Invoke(this, "ApplicationMessageSkipped: " + e.ApplicationMessage);

            mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;
           
        }

        private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            var payLoadstr = e.ApplicationMessage.ConvertPayloadToString();
            var msg = Message.FromString(payLoadstr);
            if (msg != null)
            {
                NewMessage?.Invoke(this, new Tuple<string,Message >( msg.SenderID, msg ));
            }
            else
            {
                bool garbage = false;
                var tokens = e.ApplicationMessage.Topic.Split('/');
                if (tokens.Length != 3)
                    garbage = true;
                else
                {
                    if (tokens[0].Replace("/", "") == "puzzles" &&
                        tokens[2].Replace("/", "") == "values")
                    {
                        newMeasure?.Invoke(this, new Tuple<string, string>(tokens[1], payLoadstr));
                    }
                    else
                        garbage = true;
                }
                
                if(garbage)
                    Debug?.Invoke(this, e.ApplicationMessage.Topic + ", " + e.ApplicationMessage.ConvertPayloadToString());
            }
        }

        public async void Subscrive(string channel)
        {
            await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(channel).Build());
        }

        public void Start()
        {
            try
            {
                Debug(null, $"Connecting to {ServerIP}");     
                
                var options = new ManagedMqttClientOptionsBuilder()
                                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                                    .WithClientOptions(new MqttClientOptionsBuilder()
                                        .WithClientId("Client1")
                                        .WithTcpServer(ServerIP)
                                        .Build())
                                    .Build();
                mqttClient.StartAsync(options).Wait();
                
            }
            catch
            {
                Debug(this, " + Cannot connect !!!!");
            }
        }

        public void PublishRAW(string rawValue, string topic)
        {
            Publish(rawValue, topic);
        }

        public void Publish(Message m, string topic = "master")
        {
            Publish(m.Serialize(), topic);
        }

        public async void Publish(string msg, string topic = "master")
        {
            if ((DateTime.Now - lastMessageTime).TotalMilliseconds > 50)
            {
                try
                {
                    var message = new MqttApplicationMessageBuilder()
                        .WithTopic(topic)
                        .WithPayload(msg)
                        .WithAtLeastOnceQoS()
                        .WithRetainFlag()
                        .Build();

                    await mqttClient.PublishAsync(message);
                    lastMessageTime = DateTime.Now;
                }
                catch (Exception e)
                {
                    Error(this, e);
                }

            }
        }
    }
}
