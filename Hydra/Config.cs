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
using TShockAPI.Hooks;

namespace Hydra
{
    public class Config
    {
        public enum DebugLevel
        {
            Critical = 0,
            Info = 1,
            Error = 2,
            All = 3,
            Unsecure = 4
        }
        public static readonly bool isMobileServer = Main.maxPlayers == 16 ? true : false;
        public bool EnhanceCommands = true;
        public bool ShowHydraMotd = true;
        public bool MultiLanguageSupport = true;
        public string DefaultLanguage = "English";
        public bool ForceDefaultLanguage = false;
        public string logPath = "logs";
        public DebugLevel debugLevel =
#if DEBUG
        (DebugLevel)3;
#else
			(DebugLevel)1;
#endif
        public string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
        public string DateTimeFormatLogFile = "dd-MM-yyyy--HH-mm-ss";
        public bool IsDebug =
#if DEBUG
        true;
#else
		false;
#endif
        //public static void Initialize()
        //{
        //	Read();
        //}
        public static void OnReloadEvent(ReloadEventArgs args)
        {
            Read();
        }
        public static bool Read(bool FirstLoad = false)
        {
            bool Return = false;
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

                Base.Config = ValidLang(config);

                Base.Config.logPath = Path.Combine(Base.SavePath, Base.Config.logPath);
                if (!Directory.Exists(Base.Config.logPath))
                    Directory.CreateDirectory(Base.Config.logPath);

                Logger.doLog("Hydra configuration has been loaded successfully!", DebugLevel.Info);
            }
            catch (Exception e)
            {
                Base.Config = new Config();

                if (FirstLoad)
                {
                    Logger.WriteLine($"There was an critical error loading the Hydra configuration file, using default configuration. => {e}", ConsoleColor.DarkRed);
                    Console.ReadKey();
                    Environment.Exit(-1);
                }
                Logger.doLog($"There was an error loading the Hydra configuration file, using default configuration. => {e.Message}", DebugLevel.Critical);
                Return = false;
            }
            return Return;
        }
        internal static Config ValidLang(Config config)
        {
            try
            {
                config.DefaultLanguage = Enum.Parse(typeof(TSPlayerB.Language), config.DefaultLanguage).ToString();
            }
            catch
            {
                Logger.WriteLine("Incorrect DefaultLanguage in Hydra ConfigFile, using default Hydra language", ConsoleColor.DarkRed);
                config.DefaultLanguage = TSPlayerB.Language.English.ToString();
            }
            return config;
        }
    }
}
