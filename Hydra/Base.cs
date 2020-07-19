using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;

namespace Hydra
{
    public class Base
    {
        public static readonly Version Version = new Version(1, 0, 3, 0);
        public static readonly string SavePath = Path.Combine(TShock.SavePath, "Hydra");
        public static Config Config;
        public static bool isDisposed { get; set; } = false;
        public static void OnHydraInitialize(EventArgs args)
        {
            Config.Read(true);
            Console.WriteLine($"Base / OnHydraInitialize => DebugLevel: {Config.debugLevel}");
        }
    }
}
