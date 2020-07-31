using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

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
        public static bool[] IsDisabledForSSC { get; set; } = new bool[Main.maxPlayers];
        public static void ResetArrayOnLeave(LeaveEventArgs args)
        {
            isMobile[args.Who] = false;
            PlayerLanguage[args.Who] = new Language();
            HCountry[args.Who] = null;
            ClientVersion[args.Who] = null;
            IsDisabledForSSC[args.Who] = false;
        }
        public static void SendWarningMessage(int index, string DefaultMessage, string PortugueseMessage = null, string SpanishMessage = null, string EnglishMessageIfNotDefault = null)
        {
            if (Base.Config.MultiLanguageSupport && index >= 0)
                switch (PlayerLanguage[index])
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault == null ? DefaultMessage : EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            else
                switch (Base.CurrentHydraLanguage)
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault == null ? DefaultMessage : EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            if (index >= 0)
                TShockB.Players[index].SendWarningMessage(DefaultMessage);
            else
                TSPlayer.Server.SendWarningMessage(DefaultMessage);
        }
        public static void SendInfoMessage(int index, string DefaultMessage, string PortugueseMessage = null, string SpanishMessage = null, string EnglishMessageIfNotDefault = null)
        {
            if (Base.Config.MultiLanguageSupport && index >= 0)
                switch (PlayerLanguage[index])
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault == null ? DefaultMessage : EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            else
                switch (Base.CurrentHydraLanguage)
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault == null ? DefaultMessage : EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            if (index >= 0)
                TShockB.Players[index].SendInfoMessage(DefaultMessage);
            else
                TSPlayer.Server.SendInfoMessage(DefaultMessage);
        }
        public static void SendErrorMessage(int index, string DefaultMessage, string PortugueseMessage = null, string SpanishMessage = null, string EnglishMessageIfNotDefault = null)
        {
            if (Base.Config.MultiLanguageSupport && index >= 0)
                switch (PlayerLanguage[index])
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault == null ? DefaultMessage : EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            else
                switch (Base.CurrentHydraLanguage)
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault == null ? DefaultMessage : EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            if (index >= 0)
                TShockB.Players[index].SendErrorMessage(DefaultMessage);
            else
                TSPlayer.Server.SendErrorMessage(DefaultMessage);
        }
        public static void SendSuccessMessage(int index, string DefaultMessage, string PortugueseMessage = null, string SpanishMessage = null, string EnglishMessageIfNotDefault = null)
        {
            if (Base.Config.MultiLanguageSupport && index >= 0)
                switch (PlayerLanguage[index])
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault == null ? DefaultMessage : EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            else
                switch (Base.CurrentHydraLanguage)
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault == null ? DefaultMessage : EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            if (index >= 0)
                TShockB.Players[index].SendSuccessMessage(DefaultMessage);
            else
                TSPlayer.Server.SendSuccessMessage(DefaultMessage);
        }
        public static void SendMessage(int index, string DefaultMessage, Color color, string PortugueseMessage = null, string SpanishMessage = null, string EnglishMessageIfNotDefault = null)
        {
            if (Base.Config.MultiLanguageSupport && index >= 0)
                switch (PlayerLanguage[index])
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault == null ? DefaultMessage : EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            else
                switch (Base.CurrentHydraLanguage)
                {
                    case Language.Portuguese:
                        DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                        break;
                    case Language.Spanish:
                        DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                        break;
                    case Language.English:
                        DefaultMessage = EnglishMessageIfNotDefault == null ? DefaultMessage : EnglishMessageIfNotDefault;
                        break;
                    default:
                        //Thrown NotImplementedException?
                        break;
                }
            if (index >= 0)
                TShockB.Players[index].SendMessage(DefaultMessage, color);
            else
                TSPlayer.Server.SendMessage(DefaultMessage, color);
        }
        public static void SendMessage(int index, string DefaultMessage, byte r, byte g, byte b, string PortugueseMessage = null, string SpanishMessage = null, string EnglishMessageIfNotDefault = null)
        {
            try
            {
                if (Base.Config.MultiLanguageSupport && index >= 0)
                    switch (PlayerLanguage[index])
                    {
                        case Language.Portuguese:
                            DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                            break;
                        case Language.Spanish:
                            DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                            break;
                        case Language.English:
                            DefaultMessage = EnglishMessageIfNotDefault == null ? DefaultMessage : EnglishMessageIfNotDefault;
                            break;
                        default:
                            //Thrown NotImplementedException?
                            break;
                    }
                else
                    switch (Base.CurrentHydraLanguage)
                    {
                        case Language.Portuguese:
                            DefaultMessage = PortugueseMessage == null ? DefaultMessage : PortugueseMessage;
                            break;
                        case Language.Spanish:
                            DefaultMessage = SpanishMessage == null ? DefaultMessage : SpanishMessage;
                            break;
                        case Language.English:
                            DefaultMessage = EnglishMessageIfNotDefault == null ? DefaultMessage : EnglishMessageIfNotDefault;
                            break;
                        default:
                            //Thrown NotImplementedException?
                            break;
                    }
                Color color = new Color(r, g, b);
                if (index >= 0)
                    TShockB.Players[index].SendMessage(DefaultMessage, color);
                else
                    TSPlayer.Server.SendMessage(DefaultMessage, color);
            }
            catch (Exception ex) { Logger.doLog(ex.ToString(), Config.DebugLevel.Critical, Base.Name); }
        }

        /// <summary>
        /// Sends the text of a given file to the player. Replacement of %map% and %players% if in the file.
        /// </summary>
        /// <param name="file">Filename relative to <see cref="TShock.SavePath"></see></param>
        public static void SendFileTextAsMessage(int index, string file)
        {
            string foo = "";
            //bool containsOldFormat = false;
            using (var tr = new StreamReader(file))
            {
                Color lineColor;
                while ((foo = tr.ReadLine()) != null)
                {
                    lineColor = Color.White;
                    if (string.IsNullOrWhiteSpace(foo))
                    {
                        continue;
                    }

                    var players = new List<string>();

                    foreach (TSPlayer ply in TShock.Players)
                    {
                        if (ply != null && ply.Active)
                        {
                            players.Add(ply.Name);
                        }
                    }

                    foo = foo.Replace("%map%", (TShock.Config.UseServerName ? TShock.Config.ServerName : Main.worldName));
                    foo = foo.Replace("%players%", String.Join(",", players));

                    SendMessage(index, foo, lineColor);
                }
            }
        }
    }
}
