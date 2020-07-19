using Hydra.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;

namespace Hydra
{
    public class Config
    {
		public enum DebugLevel
        {
			Critical = 0,
			Info = 1,
			Error = 2, 
			All = 3
        }
        public static readonly bool isMobileServer = Main.maxPlayers == 16 ? true : false;
        public bool MultiLanguageSupport = true;
        public string DefaultLanguage = "English";
		public bool ForceDefaultLanguage = false;
		public string logPath { get; set; } = Path.Combine(Base.SavePath, "logs");
		public DebugLevel debugLevel =
#if DEBUG
        (DebugLevel)3;
#else
			(DebugLevel)1;
#endif
		public string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
		public string DateTimeFormatLogFile = "dd-MM-yyyy--HH-mm-ss";
		//public static void Initialize()
		//{
		//	Read();
		//}
		public static bool Read(bool FirstLoad = false)
		{
			bool Return = false;
			Exception ex;
			try
			{
				if (!Directory.Exists(Base.SavePath))
					Directory.CreateDirectory(Base.SavePath);
				string filepath = Path.Combine(Base.SavePath, "HydraConfig.json");

				Config config = new Config();

				if (File.Exists(filepath))
				{
					config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(filepath));
				}

				File.WriteAllText(filepath, JsonConvert.SerializeObject(config, Formatting.Indented));
				Base.Config = config;

				Base.Config.logPath = Path.Combine(Base.SavePath, Base.Config.logPath);
				if (!Directory.Exists(Base.Config.logPath))
					Directory.CreateDirectory(Base.Config.logPath);

				Logger.doLog("Hydra configuration has been loaded successfully!", DebugLevel.All);
			}
			catch (Exception e)
			{
				ex = e;
				Base.Config = new Config();

				if (FirstLoad)
                {
					Logger.WriteLine($"There was an critical error loading the Hydra configuration file, using default configuration. => {e}", ConsoleColor.DarkRed);
					Console.ReadKey();
					Environment.Exit(-1);
				}
				Logger.doLog("There was an error loading the Hydra configuration file, using default configuration. => {e}", DebugLevel.Critical);
				Return = false;
			}
			return Return;
		}
	}
}
