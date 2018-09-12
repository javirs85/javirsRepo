using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace gameSystem
{
    public class Utils
    {
        public static string serverIP = "192.168.137.1";

        public static int baseUDPPort = 60100;
        public static int devicesUDPPort = 60101;
        public static int devicesTCPPort = 60201;

        public enum PuzzleKinds { sensor, motor, temperature, button, server }
        public enum PuzzleStatus { unsolved, solved, unset };

        public enum MessageTypes { showup, present, reset, forceSolve, debug, error, update };

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
             
             */

        public static string GetLocalIp()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp?.NetworkAdapter == null) return null;
            var hostname =
                NetworkInformation.GetHostNames()
                    .SingleOrDefault(
                        hn =>
                            hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                            == icp.NetworkAdapter.NetworkAdapterId);

            // the ip address
            return hostname?.CanonicalName;
        }
    }
}