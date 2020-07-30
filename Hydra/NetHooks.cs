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

namespace Hydra
{
    public class NetHooks
    {
        public static void OnGetData(GetDataEventArgs e)
        {
            if (e == null || Base.isDisposed || e.Handled)
                return;

            TSPlayer player = TShockB.Players[e.Msg.whoAmI];
            if (player == null)
                return;

            Logger.doLog(e.MsgID.ToString(), Config.DebugLevel.Unsecure);

            switch (e.MsgID)
            {
                case PacketTypes.ItemOwner:
                    int itemIndex = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
                    int newOwnerPlayerIndex = e.Msg.readBuffer[e.Index + 2];
                    if (itemIndex == 400)
                        if (newOwnerPlayerIndex == 16)
                            TSPlayerB.isMobile[e.Msg.whoAmI] = true;
                        else
                            TSPlayerB.isMobile[e.Msg.whoAmI] = false;
                    break;
                case PacketTypes.ContinueConnecting2:
                    e.Handled = HandleConnecting(TShockB.Players[e.Msg.whoAmI]);
                    break;
            }
        }
        #region HandleConnecting / ContinueConnecting2
        private static bool HandleConnecting(TSPlayer Player)
        {
            var user = TShock.Users.GetUserByName(Player.Name);
            Player.DataWhenJoined = new PlayerData(Player);
            Player.DataWhenJoined.CopyCharacter(Player);

            if (user != null && !TShock.Config.DisableUUIDLogin)
            {
                if (user.UUID == Player.UUID)
                {
                    if (Player.State == 1)
                        Player.State = 2;
                    NetMessage.SendData((int)PacketTypes.WorldInfo, Player.Index, -1, "");

                    Player.PlayerData = TShock.CharacterDB.GetPlayerData(Player, user.ID);

                    var group = TShock.Utils.GetGroup(user.Group);

                    Player.Group = group;
                    Player.tempGroup = null;
                    Player.User = user;
                    Player.IsLoggedIn = true;
                    Player.IgnoreActionsForInventory = "none";

                    if (Main.ServerSideCharacter)
                    {
                        if (Player.HasPermission(TShockAPI.Permissions.bypassssc))
                        {
                            Player.PlayerData.CopyCharacter(Player);
                            TShock.CharacterDB.InsertPlayerData(Player);
                        }
                        Player.PlayerData.RestoreCharacter(Player);
                    }
                    Player.LoginFailsBySsi = false;

                    if (Player.HasPermission(TShockAPI.Permissions.ignorestackhackdetection))
                        Player.IgnoreActionsForCheating = "none";

                    if (Player.HasPermission(TShockAPI.Permissions.usebanneditem))
                        Player.IgnoreActionsForDisabledArmor = "none";

                    Logger.doLogLang(DefaultMessage: $"'{Player.Name}' authenticated successfully as user '{user.Name}'", Config.DebugLevel.Info, (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Base.Config.DefaultHydraLanguage),
                                     PortugueseMessage: $"'{Player.Name}' autenticou-se com sucesso com o usuário '{user.Name}'",
                                     SpanishMessage: $"'{Player.Name}' autenticado exitosamente como usuario '{user.Name}'");
                    TShockAPI.Hooks.PlayerHooks.OnPlayerPostLogin(Player);
                    return true;
                }
            }
            else if (user != null && !TShock.Config.DisableLoginBeforeJoin)
            {
                Player.RequiresPassword = true;
                NetMessage.SendData((int)PacketTypes.PasswordRequired, Player.Index, -1, "");
                return true;
            }
            else if (!string.IsNullOrEmpty(TShock.Config.ServerPassword))
            {
                Player.RequiresPassword = true;
                NetMessage.SendData((int)PacketTypes.PasswordRequired, Player.Index, -1, "");
                return true;
            }

            if (Player.State == 1)
                Player.State = 2;
            NetMessage.SendData((int)PacketTypes.WorldInfo, Player.Index, -1, "");

            return true;
        }
        internal static void PostHandleConnecting(TSPlayer Player)
        {

        }
        #endregion
        #region GreetPlayerEventArgs
        public static void OnGreetPlayer(GreetPlayerEventArgs args)
        {
            var player = TShockB.Players[args.Who];
            if (player == null)
            {
                args.Handled = true;
                return;
            }

            string playername = $"[c/4747BF:{player.Name}]";
            if (!Main.player[player.Index].Male)
                playername = $"[c/800080:{player.Name}]";

            string via = "[c/ff4500:(via PC)]";
            if (TSPlayerB.isMobile[player.Index])
                via = "[c/ff198d:(via Mobile)]";
            string country = $" ({player.Country})";
            if (country.Contains("N/A") || !TShock.Config.EnableGeoIP)
                country = string.Empty;
            if (!player.SilentJoinInProgress)
                TShockB.AllSendMessage(DefaultMessage: $"{playername}{country} has joined the server {via}! [{TSPlayerB.PlayerLanguage[player.Index]}]", Color.White,
                                       PortugueseMessage: $"{playername}{country} entrou no servidor {via}! [{TSPlayerB.PlayerLanguage[player.Index]}]",
                                       SpanishMessage: $"{playername}{country} entró al servidor {(via.Contains("(via PC)") ? "[c/ff4500:(a través de PC)]" : "[c/ff198d:(vía teléfono móvil)]")}! [{TSPlayerB.PlayerLanguage[player.Index]}]", ignore: player);

            Logger.doLogLang(DefaultMessage: $"{player.Name}{country} has joined the server {(TSPlayerB.isMobile[player.Index] ? "(via Mobile)" : "(via PC)")}!", Config.DebugLevel.Info, Base.CurrentHydraLanguage,
                             PortugueseMessage: $"{player.Name}{country} entrou no servidor {(TSPlayerB.isMobile[player.Index] ? "(via Mobile)" : "(via PC)")}!",
                             SpanishMessage: $"{player.Name}{country} entró al servidor {(TSPlayerB.isMobile[player.Index] ? "(vía teléfono móvil)" : "(a través de PC)")}!");

            if (TShock.Config.DisplayIPToAdmins)
                TShock.Utils.SendLogs(string.Format("{0} IP => {1}", player.Name, player.IP), Color.Blue);

            TSPlayerB.SendFileTextAsMessage(player.Index, FileToolsB.MotdPath);

            if (Base.Config.ShowHydraMotd)
                TSPlayerB.SendMessage(player.Index, Base.PrintHydraMotd(TSPlayerB.PlayerLanguage[player.Index]), Color.IndianRed);

            string pvpMode = TShock.Config.PvPMode.ToLowerInvariant();
            if (pvpMode == "always")
            {
                player.TPlayer.hostile = true;
                player.SendData(PacketTypes.TogglePvp, "", player.Index);
                TSPlayer.All.SendData(PacketTypes.TogglePvp, "", player.Index);
            }

            if (!player.IsLoggedIn)
            {
                string Specifier = TShockAPI.Commands.Specifier;
                if (Main.ServerSideCharacter)
                {
                    TSPlayerB.IsDisabledForSSC[player.Index] = true;
                    TSPlayerB.SendErrorMessage(player.Index, DefaultMessage: $"Server side characters is enabled!\nPlease [c/ffd700:{Specifier}register] or [c/ffd700:{Specifier}login] to play!",
                                                             PortugueseMessage: $"SSC está ativo neste servidor.\nEfetue [c/ffd700:{Specifier}registro] ou [c/ffd700:{Specifier}login] para jogar.",
                                                             SpanishMessage: $"SSC está activo en este servidor.\nInicia [c/ffd700:{Specifier}sesión] o [c/ffd700:{Specifier}registrarse] para jugar");
                    player.LoginHarassed = true;
                }
                else if (TShock.Config.RequireLogin)
                {
                    TSPlayerB.SendErrorMessage(player.Index, DefaultMessage: $"Please [c/ffd700:{Specifier}register] or [c/ffd700:{Specifier}login] to play!",
                                                             PortugueseMessage: $"Efetue [c/ffd700:{Specifier}registro] ou [c/ffd700:{Specifier}login] para jogar.",
                                                             SpanishMessage: $"Inicia [c/ffd700:{Specifier}sesión] o [c/ffd700:{Specifier}registrarse] para jugar");
                    player.LoginHarassed = true;
                }
            }

            player.LastNetPosition = new Vector2(Main.spawnTileX * 16f, Main.spawnTileY * 16f);

            if (TShock.Config.RememberLeavePos && (TShock.RememberedPos.GetLeavePos(player.Name, player.IP) != Vector2.Zero) && !player.LoginHarassed)
            {
                player.RPPending = 1;
                TSPlayerB.SendInfoMessage(player.Index, DefaultMessage: "You will be teleported to your last known location...",
                                                        PortugueseMessage: "Você será teleportado para sua última localização...",
                                                        SpanishMessage: "Serás teletransportado a tu última ubicación...");
            }

            args.Handled = true;
        }
        #endregion
        #region LeaveEventArgs
        public static void OnLeave(LeaveEventArgs args)
        {
            if (args.Who >= TShockB.Players.Length || args.Who < 0)
            {
                //Something not right has happened
                return;
            }

            var tsplr = TShockB.Players[args.Who];

            if (tsplr != null && tsplr.ReceivedInfo)
            {
                TShockB.Players[args.Who] = null;
                TSPlayerB.ResetArrayOnLeave(args);

                if (!tsplr.SilentKickInProgress && tsplr.State >= 3)
                {
                    string playername = $"[c/4747BF:{tsplr.Name}]";
                    if (!tsplr.TPlayer.Male)
                        playername = $"[c/800080:{tsplr.Name}]";
                    TShockB.AllSendMessage(DefaultMessage: $"{playername} has left the server.", Color.DarkGray,
                                           PortugueseMessage: $"{playername} saiu do servidor.",
                                           SpanishMessage: $"{playername} dejó el servidor.");
                }

                Logger.doLogLang(DefaultMessage: $"{tsplr.Name} has left the server.", Config.DebugLevel.Info, Base.CurrentHydraLanguage,
                                 PortugueseMessage: $"{tsplr.Name} saiu do servidor.",
                                 SpanishMessage: $"{tsplr.Name} dejó el servidor.");

                if (tsplr.IsLoggedIn && !tsplr.IgnoreActionsForClearingTrashCan && Main.ServerSideCharacter && (!tsplr.Dead || tsplr.TPlayer.difficulty != 2))
                {
                    tsplr.PlayerData.CopyCharacter(tsplr);
                    TShock.CharacterDB.InsertPlayerData(tsplr);
                }

                if (TShock.Config.RememberLeavePos && !tsplr.LoginHarassed)
                {
                    TShock.RememberedPos.InsertLeavePos(tsplr.Name, tsplr.IP, (int)(tsplr.X / 16), (int)(tsplr.Y / 16));
                }

                if (tsplr.tempGroupTimer != null)
                {
                    tsplr.tempGroupTimer.Stop();
                }
            }

            // Fire the OnPlayerLogout hook too, if the player was logged in and they have a TSPlayer object.
            if (tsplr != null && tsplr.IsLoggedIn)
            {
                TShockAPI.Hooks.PlayerHooks.OnPlayerLogout(tsplr);
            }

            // The last player will leave after this hook is executed.
            if (UtilsB.ActivePlayers() == 1)
            {
                if (TShock.Config.SaveWorldOnLastPlayerExit)
                    SaveManagerB.Instance.SaveWorld();
                TShockB.SetConsoleTitle(true);
            }
        }
        #endregion
        #region ServerChatEventArgs
        public static void OnChat(ServerChatEventArgs args)
        {

        }
        #endregion
    }
}
