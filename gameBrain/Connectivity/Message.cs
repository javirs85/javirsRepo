using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameSystem
{
    public class Message
    {
        public int Id;
        public string Name;
        public Utils.MessageTypes msgType;
        public Dictionary<string, string> data;
        public Utils.PuzzleStatus Status;
        public Utils.PuzzleKinds PuzleKind;
        public string Details;
        public string IPSender;

        public Message() { } 

        public Message(Puzzle p)
        {
            this.PreFilleWithPuzzleInfo(p);
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Message Deserialize(string str)
        {
            return JsonConvert.DeserializeObject<Message>(str);
        }

        public void PreFilleWithPuzzleInfo(Puzzle p)
        {
            this.Id = p.ID;
            this.Name = p.Name;
            this.Status = p.Status;
            this.PuzleKind = p.kind;
        }

        public Puzzle getPuzzle()
        {
            Puzzle p = new Puzzle(IPSender, PuzleKind)
            {
                Details = Details,
                ID = Id,
                IP = IPSender,
                kind = PuzleKind,
                Name = Name,
                Status = Status
            };

            return p;
        }
    }
}
