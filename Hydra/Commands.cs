using Hydra.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;
using TShockAPI.DB;
using TShockAPI.Hooks;

namespace Hydra
{
    public class Commands
    {
        public static List<string> ReplacedCmds = new List<string>
        {
            "sync",
            "register",
            "password",
            "logout",
            "login"
        };
        public static string Specifier = TShockAPI.Commands.Specifier;
        public static string SilentSpecifier = TShockAPI.Commands.SilentSpecifier;
        public static void InitializeCmds(EventArgs args)
        {
            if (Base.Config.EnhanceCommands == false)
                return;
            //foreach (var tscmd in TShockAPI.Commands.ChatCommands.Where(p => p != null))
            //    foreach (string toReplace in ReplacedCmds)
            //        if (tscmd.Names.Contains(toReplace))
            //            TShockAPI.Commands.ChatCommands.Remove(tscmd);

            foreach (string toReplace in ReplacedCmds)
                for (int i = 0; i < TShockAPI.Commands.ChatCommands.Count; i++)
                    if (TShockAPI.Commands.ChatCommands[i].Names.Contains(toReplace))
                        TShockAPI.Commands.ChatCommands.Remove(TShockAPI.Commands.ChatCommands[i]);

            //Example
            //TShockAPI.Commands.ChatCommands.Add(new Command(SyncLocalArea, "EnglishName", "PortugueseName", "SpanishName", "OthersAliases")
            //{
            //    HelpText = "Yet Not Translate"
            //});

            TShockAPI.Commands.ChatCommands.Add(new Command(SyncLocalArea, "sync")
            {
                AllowServer = false,
                HelpText = "Sends all tiles from the server to the player to resync the client with the actual world state."
            });
            #region AccountCommands
            TShockAPI.Commands.ChatCommands.Add(new Command(TShockAPI.Permissions.canregister, RegisterUser, "register", "registrar", "registrarse", "registro", "cadastrar")
            {
                AllowServer = false,
                DoLog = false,
                HelpText = "Registers you an account."
            });
            TShockAPI.Commands.ChatCommands.Add(new Command(TShockAPI.Permissions.canchangepassword, PasswordUser, "password", "senha", "contraseña", "contrasena", "pw")
            {
                AllowServer = false,
                DoLog = false,
                HelpText = "Changes your account's password."
            });
            TShockAPI.Commands.ChatCommands.Add(new Command(TShockAPI.Permissions.canlogout, Logout, "logout", "logoff", "desconectarse", "sair", "desconectar")
            {
                AllowServer = false,
                HelpText = "Logs you out of your current account."
            });
            TShockAPI.Commands.ChatCommands.Add(new Command(TShockAPI.Permissions.canlogin, AttemptLogin, "login")
            {
                AllowServer = false,
                DoLog = false,
                HelpText = "Logs you into an account."
            });
            TShockAPI.Commands.ChatCommands.Add(new Command(ChangeLanguage, "lang")
            {
                AllowServer = false,
                DoLog = true
            });
            #endregion
        }
        private static void ChangeLanguage(CommandArgs args)
        {
            if (args.Parameters.Count > 0)
                switch (args.Parameters[0].ToLowerInvariant())
                {
                    case "portuguese":
                    case "portugues":
                    case "português":
                    case "portugués":
                        TSPlayerB.PlayerLanguage[args.Player.Index] = TSPlayerB.Language.Portuguese;
                        TSPlayerB.SendSuccessMessage(args.Player.Index, DefaultMessage: "Idioma alterado para [c/ffa500:Português] com sucesso!");
                        return;
                    case "ingles":
                    case "nglish":
                    case "eglish":
                    case "english":
                    case "inglés":
                    case "inglês":
                        TSPlayerB.PlayerLanguage[args.Player.Index] = TSPlayerB.Language.English;
                        TSPlayerB.SendSuccessMessage(args.Player.Index, DefaultMessage: "Language successfully changed to [c/ffa500:English]!");
                        return;
                    case "espanhol":
                    case "spanish":
                    case "espanol":
                    case "español":
                        TSPlayerB.PlayerLanguage[args.Player.Index] = TSPlayerB.Language.Spanish;
                        TSPlayerB.SendSuccessMessage(args.Player.Index, DefaultMessage: "Idioma exitosamente cambiado a [c/ffa500:Español]!");
                        return; 
                }

            TSPlayerB.SendMessage(args.Player.Index, DefaultMessage: "Player Language - Commands", Color.Magenta,
                                                     PortugueseMessage: "Idioma do jogador - Comandos",
                                                     SpanishMessage: "Idioma del jugador - Comandos");

            TSPlayerB.SendMessage(args.Player.Index, DefaultMessage: "[c/ffd700:/lang English] => Change your language to English", Color.LightGray);
            TSPlayerB.SendMessage(args.Player.Index, DefaultMessage: "[c/ffd700:/lang Português] => Altera seu idioma para português", Color.LightGray);
            TSPlayerB.SendMessage(args.Player.Index, DefaultMessage: "[c/ffd700:/lang Español] => Cambia tu idioma a Español", Color.LightGray);
            //ShowCmds(args);
        }
        private static void SyncLocalArea(CommandArgs args)
        {
            args.Player.SendTileSquare((int)args.Player.TileX, (int)args.Player.TileY, 32);
            TSPlayerB.SendWarningMessage(args.Player.Index, DefaultMessage: "[Hydra] Sync'd!",
                                                            PortugueseMessage: "[Hydra] Sincronizado!",
                                                            SpanishMessage: "[Hydra] Sincronizado!");
        }
        #region AccountVoids
        private static void RegisterUser(CommandArgs args)
        {
            try
            {
                var user = new User();
                string echoPassword = "";
                if (args.Parameters.Count == 1)
                {
                    user.Name = args.Player.Name;
                    echoPassword = args.Parameters[0];
                    try
                    {
                        user.CreateBCryptHash(args.Parameters[0]);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        TSPlayerB.SendErrorMessage(args.Player.Index, DefaultMessage: $"Password must be greater than or equal to [c/FFA500:{TShock.Config.MinimumPasswordLength}] characters.",
                                                                      PortugueseMessage: $"Sua senha deve ter ao menos [c/FFA500:{TShock.Config.MinimumPasswordLength}] caracteres.",
                                                                      SpanishMessage: $"Su contraseña debe tener al menos [c/FFA500:{TShock.Config.MinimumPasswordLength}] caracteres");
                        return;
                    }
                }
                else if (args.Parameters.Count == 2 && TShock.Config.AllowRegisterAnyUsername)
                {
                    user.Name = args.Parameters[0];
                    echoPassword = args.Parameters[1];
                    try
                    {
                        user.CreateBCryptHash(args.Parameters[1]);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        TSPlayerB.SendErrorMessage(args.Player.Index, DefaultMessage: $"Password must be greater than or equal to [c/FFA500:{TShock.Config.MinimumPasswordLength}] characters.",
                                                                      PortugueseMessage: $"Sua senha deve ter ao menos [c/FFA500:{TShock.Config.MinimumPasswordLength}] caracteres.",
                                                                      SpanishMessage: $"Su contraseña debe tener al menos [c/FFA500:{TShock.Config.MinimumPasswordLength}] caracteres");
                        return;
                    }
                }
                else
                {
                    TSPlayerB.SendErrorMessage(args.Player.Index, DefaultMessage: $"Invalid syntax! Proper syntax: [c/ffd700:{Specifier}register <new-password>]",
                                                                  PortugueseMessage: $"Uso Correto: [c/ffd700:{Specifier}registrar <nova-senha>]",
                                                                  SpanishMessage: $"¡Sintaxis inválida! Sintaxis adecuada: [c/ffd700:{Specifier}registrarse <nueva contraseña>]");
                    return;
                }

                if (Base.Config.SeparateMaleFemaleGroup && !args.Player.TPlayer.Male)
                    user.Group = Base.Config.DefaultRegistrationGroupNameFemale;
                else
                    user.Group = Base.Config.DefaultRegistrationGroupName;

                user.UUID = args.Player.UUID;

                if (TShock.Users.GetUserByName(user.Name) == null && user.Name != TSServerPlayer.AccountName) // Cheap way of checking for existance of a user
                {
                    TShock.Users.AddUser(user);
                    TSPlayerB.SendMessage(args.Player.Index, DefaultMessage: $"Account [c/ffe70c:\"{user.Name}\"] has been registered.", Color.Magenta,
                                                             PortugueseMessage: $"A conta [c/ffe70c:\"{user.Name}\"] foi registrada.",
                                                             SpanishMessage: $"Cuenta \"{user.Name}\" ha sido registrado.");

                    TSPlayerB.SendMessage(args.Player.Index, DefaultMessage: $"Your password is [c/ffa500:{echoPassword}], change it with [c/ffd700:/password].", Color.Red,
                                                             PortugueseMessage: $"Sua senha é [c/ffa500:{echoPassword}], altere-a com [c/ffd700:/senha].",
                                                             SpanishMessage: $"Tu contraseña es [c/ffa500:{echoPassword}], cambiarlo con [c/ffd700:contraseña].");

                    Logger.doLogLang(DefaultMessage: $"{args.Player.Name} registered an account: \"{user.Name}\".", Config.DebugLevel.Info, (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Base.Config.DefaultPlayerLanguage),
                                     PortugueseMessage: $"{args.Player.Name} registrou a conta: \"{user.Name}\".",
                                     SpanishMessage: $"{args.Player.Name} registrado una cuenta: \"{user.Name}\".");
                }
                else
                {
                    Logger.doLogLang(DefaultMessage: $"{args.Player.Name} failed to register an existing user: \"{user.Name}\".", Config.DebugLevel.Error, (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Base.Config.DefaultPlayerLanguage),
                                     PortugueseMessage: $"{args.Player.Name} falhou ao registrar um usuário já existente: \"{user.Name}\".",
                                     SpanishMessage: $"{args.Player.Name} error al registrar un usuario existente: \"{user.Name}\".");

                    TSPlayerB.SendMessage(args.Player.Index, DefaultMessage: $"[c/5472c0:{user.Name}] is already in use, perform [c/ffd700:/login] or use a different name.", Color.Orange,
                                                             PortugueseMessage: $"[c/5472c0:{user.Name}] já está cadastrado, efetue [c/ffd700:/login] ou use um nome diferente.",
                                                             SpanishMessage: $"[c/5472c0:{user.Name}] ya está en uso, realice [c/ffd700:/login] o usa un nombre diferente.");
                }
            }
            catch (UserManagerException ex)
            {
                Logger.doLogLang(DefaultMessage: $"RegisterUser returned an error: {ex}.", Config.DebugLevel.Critical, (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Base.Config.DefaultPlayerLanguage),
                                 PortugueseMessage: $"RegisterUser retornou um erro: {ex}.",
                                 SpanishMessage: $"RegisterUser returned an error: {ex}.");

                TSPlayerB.SendMessage(args.Player.Index, DefaultMessage: $"There was an error registering your account: {ex.Message}.", Color.Red,
                                                         PortugueseMessage: $"Ocorreu um erro ao registrar sua conta: {ex.Message}.",
                                                         SpanishMessage: $"Se produjo un error al registrar su cuenta: {ex.Message}.");
            }
        }
        private static void PasswordUser(CommandArgs args)
        {
            try
            {
                if (!args.Player.IsLoggedIn)
                {
                    TSPlayerB.SendErrorMessage(args.Player.Index, DefaultMessage: $"You need to be logged in.",
                                                                  PortugueseMessage: $"Você precisa estar logado.",
                                                                  SpanishMessage: $"Necesitas estar logueado.");
                }
                if (args.Player.IsLoggedIn && args.Parameters.Count == 1/* && args.Parameters.Count == 2*/)
                {
                    string password = args.Parameters[0];
                    //if (args.Player.User.VerifyPassword(password)) // There is no need to check the current password in my opinion, the user must be logged in anyway
                    //{
                    try
                    {
                        TShock.Users.SetUserPassword(args.Player.User, password/*args.Parameters[1]*/); // SetUserPassword will hash it for you.
                        TSPlayerB.SendSuccessMessage(args.Player.Index, DefaultMessage: $"You changed your password to: [c/ffe70c:{password}]",
                                                                        PortugueseMessage: $"Você alterou sua senha para: [c/ffe70c:{password}]",
                                                                        SpanishMessage: $"Cambiaste tu contraseña a: [c/ffe70c:{password}]");

                        Logger.doLogLang(DefaultMessage: $"{args.Player.Name} ({args.Player.IP}) changed the password of account: {args.Player.User.Name}.", Config.DebugLevel.Info, (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Base.Config.DefaultPlayerLanguage),
                                         PortugueseMessage: $"{args.Player.Name} ({args.Player.IP}) alterou a senha da conta: {args.Player.User.Name}.",
                                         SpanishMessage: $"{args.Player.Name} ({args.Player.IP}) cambió la contraseña de la cuenta: {args.Player.User.Name}.");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        TSPlayerB.SendErrorMessage(args.Player.Index, DefaultMessage: $"Password must be greater than or equal to [c/FFA500:{TShock.Config.MinimumPasswordLength}] characters.",
                                                                      PortugueseMessage: $"Sua senha deve ter ao menos [c/FFA500:{TShock.Config.MinimumPasswordLength}] caracteres.",
                                                                      SpanishMessage: $"Su contraseña debe tener al menos [c/FFA500:{TShock.Config.MinimumPasswordLength}] caracteres");
                    }
                    //}
                    //else
                    //{
                    //    TShock.Log.ConsoleInfo($"{args.Player.Name} ({args.Player.IP}) falhou ao alterar a senha da conta: {args.Player.User.Name}.");
                    //    if (args.Player.IsPortuguese)
                    //        args.Player.SendErrorMessage("A senha atual informada é inválida!");
                    //    else
                    //        args.Player.SendErrorMessage($"The current password entered is invalid!");
                    //}
                }
                else
                {
                    TSPlayerB.SendErrorMessage(args.Player.Index, DefaultMessage: $"Invalid syntax! Proper syntax: [c/ffd700:{Specifier}password <new-password>]",
                                                                  PortugueseMessage: $"Uso Correto: [c/ffd700:{Specifier}senha <nova-senha>]",
                                                                  SpanishMessage: $"¡Sintaxis inválida! Sintaxis adecuada: [c/ffd700:{Specifier}contraseña <nueva contraseña>]");

                    //args.Player.SendErrorMessage("Not logged in or invalid syntax! Proper syntax: {0}password <oldpassword> <newpassword>", Specifier);
                }
            }
            catch (UserManagerException ex)
            {
                Logger.doLogLang(DefaultMessage: $"PasswordUser returned an error: {ex}.", Config.DebugLevel.Critical, (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Base.Config.DefaultPlayerLanguage),
                                 PortugueseMessage: $"PasswordUser retornou um erro: {ex}.",
                                 SpanishMessage: $"PasswordUser returned an error: {ex}.");

                TSPlayerB.SendMessage(args.Player.Index, DefaultMessage: $"There was an error changing your account password: {ex.Message}.", Color.Red,
                                                         PortugueseMessage: $"Ocorreu um erro ao alterar a senha da sua conta: {ex.Message}.",
                                                         SpanishMessage: $"Se produjo un error al cambiar la contraseña de su cuenta. {ex.Message}.");
            }
        }

        private static void Logout(CommandArgs args)
        {
            if (!args.Player.IsLoggedIn)
            {
                TSPlayerB.SendErrorMessage(args.Player.Index, DefaultMessage: $"You need to be logged in.",
                                                              PortugueseMessage: $"Você precisa estar logado.",
                                                              SpanishMessage: $"Necesitas estar logueado.");
            }

            args.Player.Logout();

            TSPlayerB.SendSuccessMessage(args.Player.Index, DefaultMessage: $"Successful logoff. You are no longer signed in to an account",
                                                            PortugueseMessage: $"Logoff sucedido. Você não está mais conectado a uma conta",
                                                            SpanishMessage: $"Cierre de sesión exitoso. Ya no has iniciado sesión en una cuenta");
            if (Main.ServerSideCharacter)
            {
                TSPlayerB.SendWarningMessage(args.Player.Index, DefaultMessage: $"Server side characters are enabled. You need to be logged in to play.",
                                                                PortugueseMessage: $"SSC está ativado. Você precisa estar logado para jogar.",
                                                                SpanishMessage: $"SSC está habilitado. Debes iniciar sesión para jugar.");
            }
        }
        private static void AttemptLogin(CommandArgs args)
        {
            if (args.Player.LoginAttempts > TShock.Config.MaximumLoginAttempts && (TShock.Config.MaximumLoginAttempts != -1))
            {
                Logger.doLogLang(DefaultMessage: String.Format("{0} ({1}) had {2} or more invalid login attempts and was kicked automatically.", args.Player.IP, args.Player.Name, TShock.Config.MaximumLoginAttempts), Config.DebugLevel.Info, (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Base.Config.DefaultPlayerLanguage),
                                 PortugueseMessage: String.Format("{0} ({1}) teve {2} ou mais tentativas de login inválidas e foi expulso automaticamente.", args.Player.IP, args.Player.Name, TShock.Config.MaximumLoginAttempts),
                                 SpanishMessage: String.Format("{0} ({1}) tuvo {2} o más intentos de inicio de sesión no válidos y fue expulsado automáticamente.", args.Player.IP, args.Player.Name, TShock.Config.MaximumLoginAttempts));

                //To Do: Rewrite TShock.Utils.Kick to support multilanguage
                TShock.Utils.Kick(args.Player, "Multiple invalid login attempts.");
                return;
            }

            if (args.Player.IsLoggedIn)
            {
                TSPlayerB.SendErrorMessage(args.Player.Index, DefaultMessage: $"You are already logged in.",
                                                              PortugueseMessage: $"Você já está logado.",
                                                              SpanishMessage: $"Ya se ha logueado");
                return;
            }

            User user = TShock.Users.GetUserByName(args.Player.Name);
            string password = "";
            bool usingUUID = false;
            if (args.Parameters.Count == 0 && !TShock.Config.DisableUUIDLogin)
            {
                if (PlayerHooks.OnPlayerPreLogin(args.Player, args.Player.Name, ""))
                    return;
                usingUUID = true;
            }
            else if (args.Parameters.Count == 1)
            {
                if (PlayerHooks.OnPlayerPreLogin(args.Player, args.Player.Name, args.Parameters[0]))
                    return;
                password = args.Parameters[0];
            }
            else if (args.Parameters.Count == 2 && TShock.Config.AllowLoginAnyUsername)
            {
                if (String.IsNullOrEmpty(args.Parameters[0]))
                {
                    TSPlayerB.SendErrorMessage(args.Player.Index, DefaultMessage: $"Invalid Password.",
                                                                  PortugueseMessage: $"Senha inválida.",
                                                                  SpanishMessage: $"Contraseña inválida.");
                    return;
                }

                if (PlayerHooks.OnPlayerPreLogin(args.Player, args.Parameters[0], args.Parameters[1]))
                    return;

                user = TShock.Users.GetUserByName(args.Parameters[0]);
                password = args.Parameters[1];
            }
            else
            {
                //args.Player.SendErrorMessage("Syntax: {0}login - Logs in using your UUID and character name", Specifier); // Is possible to show it if UUID login is ON?

                TSPlayerB.SendMessage(args.Player.Index, DefaultMessage: $"[c/ffd700:{Specifier}login] - Command Usage", Color.Magenta,
                                                         PortugueseMessage: $"[c/ffd700:{Specifier}login] - Uso do Comando",
                                                         SpanishMessage: $"[c/ffd700:{Specifier}login] - Uso del Comando");

                TSPlayerB.SendMessage(args.Player.Index, DefaultMessage: $"        [c/ffd700:{Specifier}login <password>] - Logs in using your password and character name", Color.LightGray,
                                                         PortugueseMessage: $"        [c/ffd700:{Specifier}login <senha>] - Efetua login utilizando seu nome de usuário atual e sua senha",
                                                         SpanishMessage: $"        [c/ffd700:{Specifier}login <contraseña>] - Inicie sesión con su nombre de usuario y contraseña actuales");

                if (TShock.Config.AllowLoginAnyUsername)
                    TSPlayerB.SendMessage(args.Player.Index, DefaultMessage: $"        [c/ffd700:{Specifier}login <username> <password>] - Logs in using your username and password", Color.LightGray,
                                                             PortugueseMessage: $"        [c/ffd700:{Specifier}login <usuário> <senha>] - Efetua login utilizando um nome de usuário e uma senha",
                                                             SpanishMessage: $"        [c/ffd700:{Specifier}login <usuario> <contraseña>] - Inicia sesión con tu nombre de usuario y contraseña");

                //args.Player.SendErrorMessage("If you forgot your password, there is no way to recover it."); // It depends on Server Admin
                return;
            }
            try
            {
                if (user == null)
                {
                    TSPlayerB.SendErrorMessage(args.Player.Index, DefaultMessage: $"The informed user was not found.",
                                                             PortugueseMessage: $"O usuário informado não foi encontrado.",
                                                             SpanishMessage: $"El usuario informado no fue encontrado.");
                }
                else if (user.VerifyPassword(password) ||
                        (usingUUID && user.UUID == args.Player.UUID && !TShock.Config.DisableUUIDLogin &&
                        !String.IsNullOrWhiteSpace(args.Player.UUID)))
                {
                    args.Player.PlayerData = TShock.CharacterDB.GetPlayerData(args.Player, user.ID);

                    var group = TShock.Utils.GetGroup(user.Group);

                    args.Player.Group = group;
                    args.Player.tempGroup = null;
                    args.Player.User = user;
                    args.Player.IsLoggedIn = true;
                    args.Player.IgnoreActionsForInventory = "none";

                    if (Main.ServerSideCharacter)
                    {
                        if (args.Player.HasPermission(TShockAPI.Permissions.bypassssc))
                        {
                            args.Player.PlayerData.CopyCharacter(args.Player);
                            TShock.CharacterDB.InsertPlayerData(args.Player);
                        }
                        args.Player.PlayerData.RestoreCharacter(args.Player);
                    }
                    args.Player.LoginFailsBySsi = false;

                    if (args.Player.HasPermission(TShockAPI.Permissions.ignorestackhackdetection))
                        args.Player.IgnoreActionsForCheating = "none";

                    if (args.Player.HasPermission(TShockAPI.Permissions.usebanneditem))
                        args.Player.IgnoreActionsForDisabledArmor = "none";

                    TSPlayerB.SendSuccessMessage(args.Player.Index, DefaultMessage: $"Successfully authenticated to the user [c/5472C0:{user.Name}].",
                                                             PortugueseMessage: $"Autenticado com sucesso ao usuário [c/5472C0:{user.Name}].",
                                                             SpanishMessage: $"Autenticada con éxito a la usuaria [c/5472C0:{user.Name}].");

                    Logger.doLogLang(DefaultMessage: $"'{args.Player.Name}' authenticated successfully as user '{user.Name}'", Config.DebugLevel.Info, (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Base.Config.DefaultPlayerLanguage),
                                     PortugueseMessage: $"'{args.Player.Name}' autenticou-se com sucesso com o usuário '{user.Name}'",
                                     SpanishMessage: $"'{args.Player.Name}' autenticado exitosamente como usuario '{user.Name}'");

                    if ((args.Player.LoginHarassed) && (TShock.Config.RememberLeavePos))
                    {
                        if (TShock.RememberedPos.GetLeavePos(args.Player.Name, args.Player.IP) != Vector2.Zero)
                        {
                            Vector2 pos = TShock.RememberedPos.GetLeavePos(args.Player.Name, args.Player.IP);
                            args.Player.Teleport((int)pos.X * 16, (int)pos.Y * 16);
                        }
                        args.Player.LoginHarassed = false;

                    }
                    TShock.Users.SetUserUUID(user, args.Player.UUID);

                    PlayerHooks.OnPlayerPostLogin(args.Player);
                }
                else
                {
                    if (usingUUID && !TShock.Config.DisableUUIDLogin)
                    {
                        TSPlayerB.SendErrorMessage(args.Player.Index, DefaultMessage: $"You must enter your password!",
                                                                      PortugueseMessage: $"Você deve inserir sua senha!",
                                                                      SpanishMessage: $"Debes ingresar tu contraseña!");
                    }
                    else
                    {
                        TSPlayerB.SendErrorMessage(args.Player.Index, DefaultMessage: $"Invalid Password.",
                                                                      PortugueseMessage: $"Senha inválida.",
                                                                      SpanishMessage: $"Contraseña inválida.");
                    }

                    Logger.doLogLang(DefaultMessage: $"'{args.Player.Name}' failed to authenticate as user: '{user.Name}'", Config.DebugLevel.Info, (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Base.Config.DefaultPlayerLanguage),
                                     PortugueseMessage: $"'{args.Player.Name}' falhou ao autenticar como o usuário: '{user.Name}'",
                                     SpanishMessage: $"'{args.Player.Name}' no se pudo autenticar como usuario: '{user.Name}'");
                    args.Player.LoginAttempts++;
                }
            }
            catch (Exception ex)
            {
                args.Player.SendErrorMessage("Ocorreu um erro durante o processamento da tarefa.");
                TSPlayerB.SendErrorMessage(args.Player.Index, DefaultMessage: $"There was an error processing your request.",
                                                              PortugueseMessage: $"Houve um erro ao processar seu pedido.",
                                                              SpanishMessage: $"Hubo un error al procesar su solicitud.");

                Logger.doLog(ex.ToString(), Config.DebugLevel.Critical);
            }
        }
        #endregion
    }
}
