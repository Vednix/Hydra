using Hydra.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;
using TShockAPI.Hooks;

namespace Hydra
{
    public class Base
    {
        public static readonly Version Version = new Version(1, 0, 10, 11);
        public static readonly string SavePath = Path.Combine(TShock.SavePath, "Hydra");
        public static TSPlayerB.Language CurrentHydraLanguage { get; set; }
        public static Config Config;
        public static bool isDisposed { get; set; } = false;
        public static int ModulesLoaded { get; set; } = 0;
        public static Main game = new Main();
        public static void OnHydraInitialize(EventArgs args)
        {
            Config.Read(true);

            Assembly[] assems = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly a in assems)
                if (a.FullName.StartsWith("Hydra.") && !a.FullName.Contains("Hydra.Initializer"))
                    ModulesLoaded++;
            Logger.doLog($"Loaded {ModulesLoaded} Hydra Modules.", Config.DebugLevel.Info);
        }
        public static void OnHydraPostInitialize(EventArgs args)
        {
            Commands.InitializeCmds(args);
            Logger.WriteLine($"[Hydra] Base / OnHydraPostInitialize => DebugLevel: {Config.debugLevel}", ConsoleColor.DarkCyan);
        }
        public static string PrintHydraMotd(TSPlayerB.Language lang)
        {
            string HydraMotd = "";
            switch (lang)
            {
                case TSPlayerB.Language.English:
                    HydraMotd = $"This server is running [c/9b1cc7:Hydra (v{Version})] for TShock";
                    break;
                case TSPlayerB.Language.Portuguese:
                    HydraMotd = $"Este servidor está executando [c/9b1cc7:Hydra (v{Version})] para TShock";
                    break;
                case TSPlayerB.Language.Spanish:
                    HydraMotd = $"Este servidor se está ejecutando [c/9b1cc7:Hydra (v{Version})] para TShock";
                    break;
                default:
                    HydraMotd = $"This server is running [c/9b1cc7:Hydra (v{Version})] for TShock";
                    break;
            }
            return HydraMotd;
        }
    }
}
