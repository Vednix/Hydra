using Microsoft.Xna.Framework;
using Terraria;

namespace Hydra.Extensions
{
    public class TSPlayerB
    {
        public enum Language
        {
            English = 0,
            Portuguese = 1,
            Spanish = 2,
            Other = 69
        }
        public static bool[] isMobile { get; set; } = new bool[Main.maxPlayers];
        public static Language[] PlayerLanguage { get; set; } = new Language[Main.maxPlayers];
        public static string[] HCountry { get; set; } = new string[Main.maxPlayers];
        public static string[] ClientVersion { get; set; } = new string[Main.maxPlayers];
        public static bool[] Pinged { get; set; } = new bool[Main.maxPlayers];
        public static bool[] PingChat { get; set; } = new bool[Main.maxPlayers];
        public static bool[] PingStatus { get; set; } = new bool[Main.maxPlayers];
        public static bool[] WarnPingOrange { get; set; } = new bool[Main.maxPlayers];
        public static bool[] WarnPingRed { get; set; } = new bool[Main.maxPlayers];
        public static string[] PingedIP { get; set; } = new string[Main.maxPlayers];
        public static void ResetPingStats(int index)
        {
            if (PingedIP[index] == TShockB.Players[index].IP)
                return;
            Pinged[index] = false;
            PingChat[index] = false;
            PingStatus[index] = false;
            WarnPingOrange[index] = false;
            WarnPingRed[index] = false;
        }
        public static void SendWarningMessage(int index, string DefaultMessage, string PortugueseMessage = null, string SpanishMessage = null, string EnglishMessageIfNotDefault = null)
        {
            if (Config.MultiLanguageSupport)
                switch (PlayerLanguage[index])
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            TShockB.Players[index].SendWarningMessage(DefaultMessage);
        }
        public static void SendErrorMessage(int index, string DefaultMessage, string PortugueseMessage = null, string SpanishMessage = null, string EnglishMessageIfNotDefault = null)
        {
            if (Config.MultiLanguageSupport)
                switch (PlayerLanguage[index])
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            TShockB.Players[index].SendErrorMessage(DefaultMessage);
        }
        public static void SendSuccessMessage(int index, string DefaultMessage, string PortugueseMessage = null, string SpanishMessage = null, string EnglishMessageIfNotDefault = null)
        {
            if (Config.MultiLanguageSupport)
                switch (PlayerLanguage[index])
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            TShockB.Players[index].SendSuccessMessage(DefaultMessage);
        }
        public static void SendMessage(int index, string DefaultMessage, Color color, string PortugueseMessage = null, string SpanishMessage = null, string EnglishMessageIfNotDefault = null)
        {
            if (Config.MultiLanguageSupport)
                switch (PlayerLanguage[index])
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            TShockB.Players[index].SendMessage(DefaultMessage, color);
        }
    }
}
