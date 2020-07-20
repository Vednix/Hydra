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
        public static readonly Version Version = new Version(1, 0, 5, 0);
        public static readonly string SavePath = Path.Combine(TShock.SavePath, "Hydra");
        public static Config Config;
        public static bool isDisposed { get; set; } = false;
        public static int ModulesLoaded { get; set; } = 0;
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
    }
}
