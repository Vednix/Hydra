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
        public string DefaultHydraLanguage = "English";
        public string[] PortugueseCNCC =
        {
            "Brasil", "BR", "Portugal", "Angola", "Moçambique", "Guiné-Bissau", "Cabo Verde", "São Tomé de Príncipe", "Timor Leste", "PT", "AO", "MZ", "GW", "ST", "CV", "TL"
        };
        public string[] SpanishCNCC = // CN: https://caianomundo.ci.com.br/paises-que-falam-espanhol/#:~:text=Os%2020%20pa%C3%ADses%20que%20falam,Rep%C3%BAblica%20Dominicana%2C%20Uruguai%20e%20Venezuela. 
                                      // CC: http://manualdemarcas.inpi.gov.br/projects/manual-de-marcas-2-edicao-1-revisao/wiki/Siglas_de_pa%C3%ADses_e_organiza%C3%A7%C3%B5es
        {
            "Argentina", "AR", "Bolivia", "BO", "Chile", "CL", "Colombia", "CO", "Costa Rica", "CR", "Cuba", "CU", "Dominican Republic", "DO", "Equador", "EC", "El Salvador",
            "SV", "Guiné Equatorial", "Equatorial Guinea", "GQ", "Guatemala", "GT", "Honduras", "HN", "México", "Mexico", "MX", "Nicarágua", "Nicaragua", "NI", "Panamá", "PA",
            "Paraguai", "PY", "Peru", "PE", "Espanha", "Spain", "ES", "Uruguai", "UY", "Venezuela", "VE"
        };

        public string[] EnglishCNCC = // CN: https://pt.wikipedia.org/wiki/Lista_de_pa%C3%ADses_onde_o_ingl%C3%AAs_%C3%A9_a_l%C3%ADngua_oficial
                                      // CC: http://manualdemarcas.inpi.gov.br/projects/manual-de-marcas-2-edicao-1-revisao/wiki/Siglas_de_pa%C3%ADses_e_organiza%C3%A7%C3%B5es
        {
            "United States", "Estados Unidos", "US", "Canada", "Canadá", "CA", "Austrália", "Australia", "AU", "Maurício", "Mauritius", "MU", "New Zealand", "Nova Zelândia",
            "NZ", "United Kingdom", "Reino Unido", "GB", "Antigua and Barbuda", "AG", "Bahamas", "BS", "Barbados", "BB", "Belize", "BZ", "Dominica", "DM", "Poland", "PL", 
            "Nigeri", "NG"
        };
        public string DefaultPlayerLanguage = "English";
        public bool ForceDefaultPlayerLanguage = false;
        public bool SeparateMaleFemaleGroup = false;
        public string DefaultRegistrationGroupName = TShock.Config.DefaultRegistrationGroupName;
        public string DefaultRegistrationGroupNameFemale = "choose-default-female";
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

            Base.Config.PortugueseCNCC = Base.Config.PortugueseCNCC.Select(p => p.ToLowerInvariant()).ToArray();
            Base.Config.EnglishCNCC = Base.Config.EnglishCNCC.Select(p => p.ToLowerInvariant()).ToArray();
            Base.Config.SpanishCNCC = Base.Config.SpanishCNCC.Select(p => p.ToLowerInvariant()).ToArray();
            Base.CurrentHydraLanguage = (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Base.Config.DefaultHydraLanguage);
            return Return;
        }
        internal static Config ValidLang(Config config)
        {
            try
            {
                config.DefaultHydraLanguage = Enum.Parse(typeof(TSPlayerB.Language), config.DefaultHydraLanguage).ToString();
                config.DefaultPlayerLanguage = Enum.Parse(typeof(TSPlayerB.Language), config.DefaultPlayerLanguage).ToString();
            }
            catch
            {
                Logger.WriteLine("Incorrect DefaultLanguage in Hydra ConfigFile, using default Hydra language", ConsoleColor.DarkRed);
                config.DefaultHydraLanguage = TSPlayerB.Language.English.ToString();
                config.DefaultPlayerLanguage = TSPlayerB.Language.English.ToString();
            }
            return config;
        }
    }
}
