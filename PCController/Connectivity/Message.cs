using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connectivity
{
    public class Message
    {
        public static event EventHandler<string> SerializationError;

        public enum AvailableOrders { showup, present, forceSolve, Reset, UpdateRequested, ImSolved, UpdateYOURSolution, thisIsMySolution, statusUpdate, setThisNewSolution};
        public AvailableOrders Order;

        public string SenderID;
        public Dictionary<string, string> Params;

        public Message(AvailableOrders OrderToSend)
        {
            SenderID = "Master";
            Order = OrderToSend;
        }

        public void AddParam(string property, string value)
        {
            if (Params == null)
                Params = new Dictionary<string, string>();

            Params.Add(property, value);
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Message FromString(string serializedMessage)
        {
            try
            {
                return JsonConvert.DeserializeObject<Message>(serializedMessage);
            }
            catch
            {
                //if the message is just a float is not a broken message,is just a value and not 
                if (!float.TryParse(serializedMessage, out float f))
                    SerializationError?.Invoke(null, $"Deserialize Error. Incomming message: {serializedMessage}");
               
                return null;
            }
        }
    }
}
