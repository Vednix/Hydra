using Hydra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace Hydra
{
    public class Commands
    {
        public static List<string> ReplacedCmds = new List<string>
        {
            "sync"
        };
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

            TShockAPI.Commands.ChatCommands.Add(new Command(SyncLocalArea, "sync") 
            { 
                AllowServer = false,
                HelpText = "Sends all tiles from the server to the player to resync the client with the actual world state."
            });
        }
        private static void SyncLocalArea(CommandArgs args)
        {
            args.Player.SendTileSquare((int)args.Player.TileX, (int)args.Player.TileY, 32);
            TSPlayerB.SendWarningMessage(args.Player.Index, DefaultMessage: "[Hydra] Sync'd!",
                                                            PortugueseMessage: "[Hydra] Sincronizado!",
                                                            SpanishMessage: "[Hydra] Sincronizado!");
        }
    }
}
