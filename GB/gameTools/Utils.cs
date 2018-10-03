using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace gameTools
{
    public class Utils
    {
        

        public enum PuzzleKinds { sensor, motor, temperature, button, server }
        public enum PuzzleStatus { unsolved, solved, unset };

        public enum MessageTypes { showup, present, welcome, reset, forceSolve, debug, error, update, overwrite };

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

       
    }
}