using Newtonsoft.Json;
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
		public DebugLevel debugLevel =
			#if DEBUG 
			(DebugLevel)3;
#else
			(DebugLevel)1;
#endif
		public static Config Read()
		{
			if (!Directory.Exists(Base.SavePath))
				Directory.CreateDirectory(Base.SavePath);
			string filepath = Path.Combine(Base.SavePath, "HydraConfig.json");
			try
			{
				Config config = new Config();

				if (File.Exists(filepath))
				{
					config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(filepath));
				}

				File.WriteAllText(filepath, JsonConvert.SerializeObject(config, Formatting.Indented));
				return config;
			}
			catch (Exception ex)
			{
				//Log Exception => Loading error, using default Hydra config
				return new Config();
			}
		}
	}
}
