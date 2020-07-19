using Microsoft.Xna.Framework;
using Terraria;

namespace Hydra.Extensions
{
    public class TSPlayerB
    {
        public static bool[] isMobile { get; set; } = new bool[Main.maxPlayers];
        public static bool[] LangPortuguese { get; set; } = new bool[Main.maxPlayers];
        public static bool[] LangEnglish { get; set; } = new bool[Main.maxPlayers];
        public static string[] HCountry { get; set; } = new string[Main.maxPlayers];
        public static string[] ClientVersion { get; set; } = new string[Main.maxPlayers];
        public static bool[] Pinged { get; set; } = new bool[Main.maxPlayers];
        public static bool[] PingChat { get; set; } = new bool[Main.maxPlayers];
        public static bool[] PingStatus { get; set; } = new bool[Main.maxPlayers];
        public static bool[] WarnPingOrange { get; set; } = new bool[Main.maxPlayers];
        public static bool[] WarnPingRed { get; set; } = new bool[Main.maxPlayers];
        public static void ResetPingStats(int index)
        {
            Pinged[index] = false;
            PingChat[index] = false;
            PingStatus[index] = false;
            WarnPingOrange[index] = false;
            WarnPingRed[index] = false;
        }
        public static void SendWarningMessage(int index, string DefaultMessage, string PortugueseMessage = null)
        {
            if (Config.MultiLanguageSupport)
            {
                if (LangPortuguese[index] && PortugueseMessage != null)
                    DefaultMessage = PortugueseMessage;
            }
            TShockB.Players[index].SendWarningMessage(DefaultMessage);
        }
        public static void SendErrorMessage(int index, string DefaultMessage, string PortugueseMessage = null)
        {
            if (Config.MultiLanguageSupport)
            {
                if (LangPortuguese[index] && PortugueseMessage != null)
                    DefaultMessage = PortugueseMessage;
            }
            TShockB.Players[index].SendErrorMessage(DefaultMessage);
        }
        public static void SendMessage(int index, string DefaultMessage, Color color, string PortugueseMessage = null)
        {
            if (Config.MultiLanguageSupport)
            {
                if (LangPortuguese[index] && PortugueseMessage != null)
                    DefaultMessage = PortugueseMessage;
            }
            TShockB.Players[index].SendMessage(DefaultMessage, color);
        }
    }
}
