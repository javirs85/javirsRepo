using System;

namespace GBCore.Connectivity
{
    public class NetUtils
    {
        public static int BaseListeningPort = 53200;
        public static BrainToken Token;
    }

    /// <summary>
    /// Simplified version with few components good for sending information
    /// about the exercise to the UDP environment whithout exposing too much data
    /// </summary>
    public class SimplifiedExercise
    {
        public int ID;
        public string Name = string.Empty;
        public string Side;


        public SimplifiedExercise()
        {
            ID = 0;
            Name = "test";
            Side = "notSet";
        }

        public SimplifiedExercise(int _id, string _name, string _side)
        {
            ID = _id;
            Name = _name;
            Side = _side;
        }
    }

    public enum messageKinds { heartbeat, stopHeartbeat, present, deviceClosed, MainServerClosed, majorErrorInDevice, forceSolve, reset, update }

    

    /*
     Message structure:
        showup :  '{"msgType":0,
                    "Name":"server",
                    "Id":"0",
                    "Status":0,
                    "Details":"repeated same old details",
                    "PuzleKind":0,
                    "IPSender":"192.168.137.1"}'

        present:    '{"msgType":1,
                    "Name":"laptop",
                    "Id":"0",
                    "Status":0,
                    "Details":"repeated same old details",
                    "PuzleKind":0,
                    "IPSender":"192.168.137.7"}'

        reset:     '{ "msgType":2,
                    "Name":"laptop",
                    "Id":"0",
                    "Status":0,
                    "Details":"repeated same old details",
                    "PuzleKind":0,
                    "IPSender":"192.168.137.7"}'

        forceSolve: '{"msgType":3,
                    "Name":"laptop",
                    "Id":"0",
                    "Status":0,
                    "Details":"repeated same old details",
                    "PuzleKind":0,
                    "IPSender":"192.168.137.7"}'

        debug:     '{ "msgType":4,
                    "Name":"laptop",
                    "Id":"0",
                    "data":{
                            "debugInfo":"error Explanaton",
                            "callStack":"location details"
                            },
                    "Status":0,
                    "Details":"repeated same old details",
                    "PuzleKind":0,
                    "IPSender":"192.168.137.7"}'

        error:     '{ "msgType":5,
                    "Name":"laptop",
                    "Id":"0",
                    "data":{
                            "errorInfo":"error Explanaton",
                            "callStack":"location details"
                            },
                    "Status":0,
                    "Details":"repeated same old details",
                    "PuzleKind":0,
                    "IPSender":"192.168.137.7"}

        update:     '{"msgType":6,
                    "Name":"laptop",
                    "Id":"0",
                    "Status":0,
                    "Details":"new details",
                    "PuzleKind":0,
                    "IPSender":"192.168.137.7"}'

        overwrite:  '{"msgType":7,
                    "Name":"laptop",
                    "Id":"0",
                    "Status":0,
                    "data":Array,
                    "PuzleKind":0,
                    "IPSender":"192.168.137.7"}'

         */



    public class WrongTokenException : Exception
    {

    }

}
