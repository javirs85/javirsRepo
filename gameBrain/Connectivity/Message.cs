using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameBrain
{
    public class Message
    {
        
        public Utils.MessageTypes MessageType;

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override string ToString()
        {
            return Serialize();
        }

        public static Message FromString(string serializedObject)
        {
            return JsonConvert.DeserializeObject<Message>(serializedObject);
        }
    }
}
