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
        public event EventHandler<string> NewMessage;

        public static string ServerIP = "localhost";

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
            mqttClient.Connected += (o, e) => Debug?.Invoke(this, "Client conencted to server");
            //mqttClient.ApplicationMessageProcessed += (o, e) => Debug?.Invoke(this, "applicationMEssageProcessed: "+e.Exception.Message);
            mqttClient.ApplicationMessageSkipped += (o, e) => Debug?.Invoke(this, "ApplicationMessageSkipped: " + e.ApplicationMessage);

            mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;
           
        }

        private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            NewMessage?.Invoke(this, e.ClientId + " : " + e.ApplicationMessage.Topic + " : " + e.ApplicationMessage.ConvertPayloadToString());
        }

        public async void Subscrive(string channel)
        {
            await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(channel).Build());
        }

        public void Start()
        {
            try
            {
                var options = new ManagedMqttClientOptionsBuilder()
                                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                                    .WithClientOptions(new MqttClientOptionsBuilder()
                                        .WithClientId("Client1")
                                        .WithTcpServer(ServerIP)
                                        .Build())
                                    .Build();
                mqttClient.StartAsync(options).Wait();
                
            }
            catch (Exception e)
            {
                Debug(this, " + Cannot connect !!!!");
            }
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
                    ;
                }

            }
        }
    }
}
