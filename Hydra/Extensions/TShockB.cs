using Hydra.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace Hydra.Extensions
{
    public class TShockB
    {
        public static TSPlayer[] Players { get; protected set; } = TShock.Players;
        public static void AllSendMessage(string DefaultMessage, Color color, string PortugueseMessage = null, TSPlayer ignore = null)
        {
            foreach (TSPlayer tsplayer in TShock.Players.Where(p => p != null && p != ignore && p.Active))
            {
                TSPlayerB.SendMessage(tsplayer.Index, DefaultMessage, color, PortugueseMessage);
            }
        }
    }
}
