using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GBCore.Connectivity
{

    public class BrainMessage
    {

        #region ctor
        public BrainMessage()
        {
        }

        public BrainMessage(messageKinds kind)
        {
            this.Order = kind;
        }

        public BrainMessage(int _deviceID, messageKinds _order)
        {
            this._deviceID = _deviceID;
            this.Order = _order;
        }

        public BrainMessage(int deviceID, messageKinds _order, Dictionary<string, object> info = null)
        {
            this._deviceID = deviceID;
            this.Order = _order;
            this.Params = info;
        }

        public BrainMessage(int deviceID, messageKinds _order, string _token, Dictionary<string, object> info = null)
        {
            this._deviceID = deviceID;
            this.Order = _order;
            this.Params = info;
            this.Token = _token;
        }

        #endregion


        #region private 
        private string _order;
        private Dictionary<string, object> _params;
        private int _deviceID;
        #endregion


        #region public

        /// <summary>
        /// Represents the ID of the device sending the message
        /// </summary>
        public int Id
        {
            get
            {
                return _deviceID;
            }

            set
            {
                _deviceID = value;
            }
        }


        public messageKinds Order
        {
            set { _order = value.ToString(); }
            get { return (messageKinds)Enum.Parse(typeof(messageKinds), _order); }
        }

        public string Token { get; set; }


        public Dictionary<string, object> Params
        {
            get
            {
                return _params;
            }

            set
            {
                _params = value;
            }
        }

        #endregion

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override string ToString()
        {
            return this.Serialize();
        }

        public static BrainMessage fromString(string s)
        {
            return JsonConvert.DeserializeObject<BrainMessage>(s);
        }
    }
}
