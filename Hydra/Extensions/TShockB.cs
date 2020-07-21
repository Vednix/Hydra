using Hydra.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using static Hydra.Extensions.TSPlayerB;
namespace Hydra.Extensions
{
    public class TShockB
    {
        public static TSPlayer[] Players { get; protected set; } = TShock.Players;
        public static string[] LoggedInAccountNames { get; set; } = new string[Main.maxPlayers];
        public static void AllSendMessage(string DefaultMessage, Color color, string PortugueseMessage = null, string SpanishMessage = null, string EnglishMessageIfNotDefault = null, TSPlayer ignore = null)
        {
            Parallel.ForEach(TShock.Players.Where(p => p != null && p != ignore && p.Active), tsplayer =>
            {
                TSPlayerB.SendMessage(tsplayer.Index, DefaultMessage, color, PortugueseMessage, SpanishMessage, EnglishMessageIfNotDefault);
            });
        }
        /// <summary>SetConsoleTitle - Updates the console title with some pertinent information.</summary>
        /// <param name="empty">empty - True/false if the server is empty; determines if we should use Utils.ActivePlayers() for player count or 0.</param>
        public static void SetConsoleTitle(bool empty)
        {
            Console.Title = string.Format("{0}{1}/{2} @ {4}:{5} (TShock v{6})",
                    !string.IsNullOrWhiteSpace(TShock.Config.ServerName) ? TShock.Config.ServerName + " - " : "",
                    empty ? 0 : TShock.Utils.ActivePlayers(),
                    TShock.Config.MaxSlots, Main.worldName, Netplay.ServerIP.ToString(), Netplay.ListenPort, TShock.VersionNum);
        }
    }
}
