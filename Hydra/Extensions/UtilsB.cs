using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;

namespace Hydra.Extensions
{
    public class UtilsB
    {
        //public static TShockAPI.Utils Utils = TShock.Utils;
        /// <summary>
        /// Returns an IPv4 address from a DNS query
        /// </summary>
        /// <param name="hostname">string ip</param>
        public static string GetIPv4AddressFromHostname(string hostname)
        {
            try
            {
                //Get the ipv4 address from GetHostAddresses, if an ip is passed it will return that ip
                var ip = Dns.GetHostAddresses(hostname).FirstOrDefault(i => i.AddressFamily == AddressFamily.InterNetwork);
                //if the dns query was successful then return it, otherwise return an empty string
                return ip != null ? ip.ToString() : "";
            }
            catch (SocketException)
            {
            }
            return "";
        }

        /// <summary>
        /// Gets the number of active players on the server.
        /// </summary>
        /// <returns>The number of active players on the server.</returns>
        public static int ActivePlayers()
        {
            return Main.player.Where(p => null != p && p.active).Count();
        }
    }
}
