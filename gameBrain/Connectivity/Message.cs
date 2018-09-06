using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameBrain
{
    public class Message
    {
        public Utils.MessageTypes msgType;
        public Dictionary<string, string> data;
        public Utils.PuzzleStatus Status;
        public Utils.PuzzleKinds PuzleKind;

        public string IPSender;

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Message Deserialize(string str)
        {
            return JsonConvert.DeserializeObject<Message>(str);
        }
    }
}
