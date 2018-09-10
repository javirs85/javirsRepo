using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace gameSystem
{
    public static class gameBrain
    {
        public static event EventHandler<string> NewDebugMsgForUI;

        static WebController webServer = null;
        static UDPController Udp = null;

        static List<Puzzle> Puzzles;

        public static void Start()
        {
            webServer = new WebController();
            WebController.NewMessageFromSocket += newMessageFromWebSite;
            webServer.StartAll();

            Udp = new UDPController();
            UDPController.NewUDPmessageFromDevice += UDPController_NewUDPmessageFromDevice;
            Udp.StartListening();

            Puzzles = new List<Puzzle>();

            UDPController.SendBroadcast("SHOWUP");
        }

        #region communication

        private static void UDPController_NewUDPmessageFromDevice(object sender, Message m)
        {
            try
            {
                switch (m.msgType)
                {
                    case Utils.MessageTypes.present:
                        if (Puzzles.Any(x => x.ID == m.Id))
                            DebugErrorMsg("Tried to add a puzzle with ID: " + m.Id + " but it already esists.");
                        else
                        {
                            Puzzles.Add(m.getPuzzle());
                            webServer.Send(m.Serialize());
                        }
                        break;

                    case Utils.MessageTypes.update:
                        var puzzle = Puzzles.Find(x => x.ID == m.Id);
                        if (puzzle == null)
                            DebugErrorMsg("Tried to Update a puzzle with ID: " + m.Id + " but it does not esists.");
                        else
                        {
                            puzzle.Status = m.Status;
                            puzzle.Details = m.Details;
                            webServer.Send(m.Serialize());
                        }
                        break;

                    case Utils.MessageTypes.debug:
                        Debug("UDP from device " + m.data["debugInfo"]);
                        break;
                    case Utils.MessageTypes.error:
                        DebugErrorMsg(null, "**ERROR : UDP from device " + m.data["debugInfo"]);
                        break;
                    default:
                        DebugErrorMsg(null, "Unexpected message from device: "+ m.Name +" : " + m.msgType.ToString());
                        break;
                }
                
            }catch (Exception ex)
            {
                ;
            }
        }

        private static void UDPController_newDeviceAppeared(object sender, Message e)
        {
            Puzzle p = new Puzzle(e.IPSender, e.PuzleKind);
            webServer.Send(e.Serialize());
        }

        private static void newMessageFromWebSite(object sender, string e)
        {
            if (e == "discoveryRequest")
                UDPController.SendBroadcast("ShowUp");
            Debug(null, "Website requested:" + e);
        }

        #endregion

        #region debug

        public static void Debug(string str, bool onlyUI = false) {
            Debug(null, str, onlyUI);
        }

        public static void Debug(object o, string msg, bool onlyUI = false)
        {
            //debugs in the raspi UI
            NewDebugMsgForUI?.Invoke(o, msg);

            if (!onlyUI)
            {
                //and sends to the website via websocket
                Message debugMessage = new Message
                {
                    IPSender = Utils.serverIP,
                    msgType = Utils.MessageTypes.debug,
                    PuzleKind = Utils.PuzzleKinds.server,
                    Status = Utils.PuzzleStatus.unset,
                    Name = "server",
                    Id = 0,
                    data = new Dictionary<string, string> { { "debugInfo", "GB:" + msg }, { "callStack", "" } }
                };

                webServer.Send(debugMessage.Serialize());
            }
        }

        public static void DebugErrorMsg(string str)
        {
            DebugErrorMsg(null, str);
        }

        public static void DebugErrorMsg(object o, Exception e)
        {
            //debugs in the raspi UI
            NewDebugMsgForUI?.Invoke(o, "**ERROR GB:" + e.Message + " || Inner: " + e.InnerException?.Message);

            //and sends to the website via websocket
            Message debugMessage = new Message
            {
                IPSender = Utils.serverIP,
                msgType = Utils.MessageTypes.error,
                PuzleKind = Utils.PuzzleKinds.server,
                Status = Utils.PuzzleStatus.unset,
                Name = "server",
                Id = 0,
                data = new Dictionary<string, string> { { "errorInfo", "GB:" + e.Message + " || Inner: "+ e.InnerException?.Message}, { "callStack", e.StackTrace } }
            };

            webServer.Send(debugMessage.Serialize());
        }
        

        public static void DebugErrorMsg(object o, string msg)
        {
            //debugs in the raspi UI
            NewDebugMsgForUI?.Invoke(o, "**ERROR"+msg);

            //and sends to the website via websocket
            Message debugMessage = new Message
            {
                IPSender = Utils.serverIP,
                msgType = Utils.MessageTypes.error,
                PuzleKind = Utils.PuzzleKinds.server,
                Status = Utils.PuzzleStatus.unset,
                Name = "server",
                Id = 0,
                data = new Dictionary<string, string> { { "errorInfo", "GB:" + msg }, { "callStack", "" } }
            };

            webServer.Send(debugMessage.Serialize());
        }

        #endregion
    }
}
