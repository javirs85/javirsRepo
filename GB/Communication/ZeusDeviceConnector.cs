﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Connectivity
{
    public class ZeusDeviceConnector :IDisposable
    {
        //Not used events 
        //
        //   public event EventHandler<ZeusMessage> g_Showup;
        //   public event EventHandler<ZeusMessage> g_OpenConfig;
        //   public event EventHandler<ZeusMessage> g_New_OK_Classification;
        //   public event EventHandler<ZeusMessage> g_new_WRONG_classification;
        //   public event EventHandler<ZeusMessage> g_NewCue;
        //   public event EventHandler<ZeusMessage> g_NewFinished;
        //   public event EventHandler<ZeusMessage> g_NewFeedBack_ON;
        //   public event EventHandler<ZeusMessage> g_NewFeedback_OFF;
        //   public event EventHandler<ZeusMessage> g_NewSoftwareClosed;
        //   public event EventHandler<ZeusMessage> g_PatientChanged;

        public event EventHandler<ZeusMessage> g_Present;
        public event EventHandler<ZeusMessage> g_ConfigOk;
        public event EventHandler<ZeusMessage> g_DeviceClosed;
        public event EventHandler<ZeusMessage> g_NewErrorFromDevice;
        public static event EventHandler<Exception> g_NewNetworkError;
        public event EventHandler<ZeusMessage> g_WrongToken;

        /// <summary>
        /// the heartbeat failed
        /// </summary>
        public event EventHandler g_softwareMissing;
        public event EventHandler g_ShowSoftwareWindow;
        public event EventHandler<Exception> g_CannotConnect;

        public int remoteID { get; set; }
        public string remoteName { get; set; }
        public bool IsFullyConnected { get; set; }

        private TCPComm.TCPConnector connector;

        #region heartbeat

        private System.Timers.Timer HeartbeatTimeOut = new System.Timers.Timer(1000);
        private bool isTimeOutConfigured = false;

        public void ConfigHeartbeatTimeOut(int ms)
        {
            HeartbeatTimeOut.Interval = ms;
            HeartbeatTimeOut.Elapsed += HeartbeatTimeOut_Elapsed;
            HeartbeatTimeOut.AutoReset = false;
            HeartbeatTimeOut.Enabled = true;
            HeartbeatTimeOut.Start();

            isTimeOutConfigured = true;
        }

        public void DisableHeartbeatTimeOut()
        {
            HeartbeatTimeOut.Stop();
        }

        private void HeartbeatTimeOut_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            g_softwareMissing?.Invoke(this, EventArgs.Empty);
            HeartbeatTimeOut.Stop();
        }

        public void StartSendingHearbeat(int IdFrom)
        {
            throw new NotImplementedException();
        }

        public void StopSendingHeartbeat()
        {
            throw new NotImplementedException();
        }
        #endregion


        public ZeusDeviceConnector(TCPComm.TCPConnector _conn)
        {
            connector = _conn;
            connector.newError += (o1, err) => {
                g_NewNetworkError(this, err);
            };
            connector.newRawMessage += (o, str) =>
            {
                if (str == "")
                {
                    g_DeviceClosed?.Invoke(this, null);
                    connector.Dispose();
                    return;
                }

                var msgs = TCPComm.Utils.SplitIntoMessages(str);
                foreach (var raw in msgs)
                {
                    var msg = ZeusMessage.fromString(raw);
                    if (msg.Token != NetUtils.Token.Token)
                        g_WrongToken.Invoke(this, msg);
                    else
                    {
                        switch (msg.Order)
                        {
                            case messageKinds.heartbeat:
                                break;
                            
                            case messageKinds.present:
                                g_Present?.Invoke(this, msg);
                                break;
                            case messageKinds.showYourWindow:
                                g_ShowSoftwareWindow?.Invoke(this, EventArgs.Empty);
                                break;
                           
                            case messageKinds.config_ok:
                                g_ConfigOk?.Invoke(this, msg);
                                break;
                            case messageKinds.deviceClosed:
                                g_DeviceClosed?.Invoke(this, msg);
                                connector.Dispose();
                                break;

                            case messageKinds.majorErrorInDevice:
                                g_NewErrorFromDevice?.Invoke(this, msg);
                                break;
                            default:
                                throw new Exception($"Unnexpected Order: [{msg.Order}] received from device [{this.remoteName}]");
                        }
                    }
                }
            };
        }

        public void Send(ZeusMessage msg)
        {
            Send(msg.Serialize());
        }

        public void Send (string s)
        {
            connector.Send(s);
        }

        internal void Stop()
        {
            connector.Dispose();
        }

        public void Dispose()
        {
            this.Stop();
        }
    }
}