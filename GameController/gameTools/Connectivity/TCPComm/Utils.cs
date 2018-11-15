using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPComm
{
    public static class Utils
    {
        public static string GetLocalIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static List<string> SplitIntoMessages(string source)
        {
            try
            {
                var toReturn = new List<string>();

                int init = source.IndexOf("{\"Id");
                int end = source.IndexOf("{\"Id", init + 1);

                while (end > -1)
                {
                    var str = source.Substring(init, (end - init));
                    toReturn.Add(str);

                    init = end;
                    end = source.IndexOf("{\"Id", init + 1);
                }
                toReturn.Add(source.Substring(init, source.Length - init));

                return toReturn;
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
