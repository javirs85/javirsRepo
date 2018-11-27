using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

using TCPComm;

namespace GBCore.Connectivity
{
    public class ConnectivityCore :IDisposable
    {
        List<TCPConnector> Limbo;
        //List<Device> WorkingDevices;

        public event EventHandler<Exception> g_NewErrorFromDevice;
        public event EventHandler<ConnectionInfo> g_newDeviceConnected;
        public event EventHandler<string> g_newDebugMessage;

        private TCPServer Server;

        public ConnectivityCore(int port = 0)
        {
            if (NetUtils.Token == null || NetUtils.Token.ToString() == string.Empty)
            //NetUtils.Token = BrainToken.LoadDataFromXML();
            {
                NetUtils.Token = new BrainToken();
                NetUtils.Token.Token = "Game1";
            }

            if (port == 0)
                port = NetUtils.BaseListeningPort;

            Limbo = new List<TCPConnector>();

            try
            {
                Server = new TCPServer(port);
                Server.newError += (o, err) => {
                    g_NewErrorFromDevice.Invoke(this, err);
                };
                Server.newDeviceConnected += (sender, deviceConnector) =>
                {
                    Limbo.Add(deviceConnector);


                    deviceConnector.newRawMessage += CatchPresentMessage;
                };
                Server.newDebugMessage += (s, m) => g_newDebugMessage(s, m);
            }
            catch (Exception e3)
            {
                g_NewErrorFromDevice.Invoke(this, e3);
            }
        }

        private void CatchPresentMessage(object sender, string deviceMessage)
        {
            try
            {

                var msgs = TCPComm.Utils.SplitIntoMessages(deviceMessage);

                

                if(deviceMessage == "")
                {
                    var con = sender as TCPConnector;
                    con.Dispose();
                    return;
                }

                var msg = BrainMessage.fromString(msgs[0]);

                if (msg.Token != NetUtils.Token.Token)
                {
                    //this device is not for our system
                    g_NewErrorFromDevice(null, new Exception($"Bad Token. Expected {NetUtils.Token.Token} found {msg.Token}"));
                }
                else if (msg.Order == messageKinds.present)
                {
                    var con = sender as TCPConnector;
                    con.newRawMessage -= CatchPresentMessage;
                    Limbo.Remove(con);
                    var ZConn = new BrainConnector(con);
                    ZConn.remoteID = int.Parse(msg.Params["myID"].ToString());
                    ZConn.remoteName = msg.Params["myName"].ToString();
                    g_newDeviceConnected(this, new ConnectionInfo() {
                        ID = ZConn.remoteID,
                        Name = ZConn.remoteName,
                        Connector = ZConn,
                        Default = msg.Params["Default"]?.ToString(),
                        Solution = msg.Params["Solution"]?.ToString()
                    });
                }
                else if((msg.Order == messageKinds.deviceClosed))
                {
                    // a device was closed before teh present message was sent. We do nothing, the TCPServer layer will remove left overs.
                     return;
                }
                else
                {
                    throw new Exception($"Received a {msg.Order} message from a device that never sent us the Present message");
                }
            }
            catch (Exception e2)
            {
                g_newDebugMessage(this, e2.Message);
            }
        }
        

        public class ConnectionInfo
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Solution { get; set; }
            public string Default { get; set; }
            public BrainConnector Connector { get; set; }
        }

        //TODO: nee
        /*

        private void Listener_g_NewErrorFromDevice(object sender, NewMessageEventArgs e)
        {
            var device = Zeus.DevicesAndExercises.DevicesAndExercises.devices.Find(x => x.Id == e.Msg.Id);
            device.Status = Device.possibleStatus.waitingConfig;
            g_ErrorFromDevice?.Invoke(this, new ExceptionArgsExtended(new Exception(e.Msg.Params["Message"].ToString()), device));
        }

        private void Listener_g_ShowRecoverixWindow(object sender, EventArgs e)
        {
            devices.First().RequestRxShowWindow();
        }

        private void Listener_g_CannotConnectUDP(object sender, ExceptionArgs e)
        {
            if (g_UDPConnectionFailed != null)
                g_UDPConnectionFailed(this, new Gtec2.ExceptionArgs(e.exception));
        }


        private void Listener_g_NewDeviceClosed(object sender, NewMessageEventArgs e)
        {
            Device device = devices.Find(x => x.Id == e.Msg.Id);
            device.Status = Device.possibleStatus.unUsable;
        }

        private void Listener_g_ConfigOk(object sender, NewMessageEventArgs e)
        {
            //TODO: store parameters from the config to show them in the results
            Device device = devices.Find(x => x.Id == e.Msg.Id);
            device.Status = Device.possibleStatus.ReadyToUse;
            if (device.Id == 2) Zeus.DevicesAndExercises.DevicesAndExercises.configuredParams = e.Msg.Params;
        }

        private void Listener_g_Present(object sender, NewMessageEventArgs e)
        {
            Device device = devices.Find(x => x.Id == e.Msg.Id);
            device.PresentPending = false;
            device.UDPSender.IP = (e.Msg.Params["source_IPEndPoint"] as System.Net.IPEndPoint).Address.ToString();
            device.Status = Device.possibleStatus.notConfigured;
        }

        
        }*/

        public void Stop()
        {
            Server.Stop();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
