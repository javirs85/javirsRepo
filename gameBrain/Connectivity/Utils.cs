using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace gameBrain
{
    public class Utils
    {
        public static int baseUDPPort = 60100;
        public static int devicesUDPPort = 60101;
        public static int devicesTCPPort = 60201;

        public enum PuzzleKinds { sensor, motor, temperature, button }
        public enum MessageTypes { showup, present, reset, forceSolve };
        public enum PuzzleStatus { unsolved, solved };

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