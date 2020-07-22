using Hydra.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;

namespace Hydra
{
    public class SetLanguage
    {
        internal static string CachedCountry;
        //Moved to ConfigFile
        //private static readonly string[] PortugueseCNCC =
        //{
        //    "Brasil", "BR", "Portugal", "Angola", "Moçambique", "Guiné-Bissau", "Cabo Verde", "São Tomé de Príncipe", "Timor Leste", "PT", "AO", "MZ", "GW", "ST", "CV", "TL"
        //};
        //private static readonly string[] SpanishCNCC = // CN: https://caianomundo.ci.com.br/paises-que-falam-espanhol/#:~:text=Os%2020%20pa%C3%ADses%20que%20falam,Rep%C3%BAblica%20Dominicana%2C%20Uruguai%20e%20Venezuela. 
        //                                               // CC: http://manualdemarcas.inpi.gov.br/projects/manual-de-marcas-2-edicao-1-revisao/wiki/Siglas_de_pa%C3%ADses_e_organiza%C3%A7%C3%B5es
        //{
        //    "Argentina", "AR", "Bolivia", "BO", "Chile", "CL", "Colombia", "CO", "Costa Rica", "CR", "Cuba", "CU", "Dominican Republic", "DO", "Equador", "EC", "El Salvador",
        //    "SV", "Guiné Equatorial", "Equatorial Guinea", "GQ", "Guatemala", "GT", "Honduras", "HN", "México", "Mexico", "MX", "Nicarágua", "Nicaragua", "NI", "Panamá", "PA",
        //    "Paraguai", "PY", "Peru", "PE", "Espanha", "Spain", "ES", "Uruguai", "UY", "Venezuela", "VE"
        //};
        //private static readonly string[] EnglishCNCC = // CN: https://pt.wikipedia.org/wiki/Lista_de_pa%C3%ADses_onde_o_ingl%C3%AAs_%C3%A9_a_l%C3%ADngua_oficial
        //                                               // CC: http://manualdemarcas.inpi.gov.br/projects/manual-de-marcas-2-edicao-1-revisao/wiki/Siglas_de_pa%C3%ADses_e_organiza%C3%A7%C3%B5es
        //{
        //    "United States", "Estados Unidos", "US", "Canada", "Canadá", "CA", "Austrália", "Australia", "AU", "Maurício", "Mauritius", "MU", "New Zealand", "Nova Zelândia", 
        //    "NZ", "United Kingdom", "Reino Unido", "GB", "Antigua and Barbuda", "AG", "Bahamas", "BS", "Barbados", "BB", "Belize", "BZ", "Dominica", "DM" 
        //};
        public static void OnJoin(JoinEventArgs args)
        {
            var player = TShockB.Players[args.Who];
            if (player == null)
            {
                args.Handled = true;
                return;
            }

            player.Country = player.Country == "??" ? (GetUserCountryByIp(player.IP) == "N/A" ? "N/A" : CachedCountry) : player.Country;
            if (player.IP.StartsWith("192.") || player.IP.StartsWith("10.0") || player.IP == "127.0.0.1")
            {
                TSPlayerB.PlayerLanguage[player.Index] = (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Base.Config.DefaultHydraLanguage);
                player.Country = "Localhost";
            }
            else
            {
                //if (Base.Config.PortugueseCNCC.Where(p => p.ToLowerInvariant().Contains(player.Country.ToLowerInvariant())).ToString().Count() >= 1)
                //    TSPlayerB.PlayerLanguage[player.Index] = TSPlayerB.Language.Portuguese;
                //else if (Base.Config.EnglishCNCC.Where(p => p.ToLowerInvariant().Contains(player.Country.ToLowerInvariant())).ToString().Count() >= 1)
                //    TSPlayerB.PlayerLanguage[player.Index] = TSPlayerB.Language.English;
                //else if (Base.Config.SpanishCNCC.Where(p => p.ToLowerInvariant().Contains(player.Country.ToLowerInvariant())).ToString().Count() >= 1)
                //    TSPlayerB.PlayerLanguage[player.Index] = TSPlayerB.Language.Spanish;
                if (Base.Config.PortugueseCNCC.Contains(player.Country))
                    TSPlayerB.PlayerLanguage[player.Index] = TSPlayerB.Language.Portuguese;
                else if (Base.Config.EnglishCNCC.Contains(player.Country))
                    TSPlayerB.PlayerLanguage[player.Index] = TSPlayerB.Language.English;
                else if (Base.Config.SpanishCNCC.Contains(player.Country))
                    TSPlayerB.PlayerLanguage[player.Index] = TSPlayerB.Language.Spanish;
                else
                    try
                    {
                        TSPlayerB.PlayerLanguage[player.Index] = (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Base.Config.DefaultPlayerLanguage);
                    }
                    catch (ArgumentException ex)
                    {
                        if (Base.Config.debugLevel <= Config.DebugLevel.Info)
                            Logger.doLog("Incorrect DefaultLanguage in Hydra ConfigFile, using default Hydra language", Config.DebugLevel.Info);
                        else
                            Logger.doLog($"Incorrect DefaultLanguage in Hydra ConfigFile, using default Hydra language => Details: {ex}", Config.DebugLevel.Error);
                        TSPlayerB.PlayerLanguage[player.Index] = TSPlayerB.Language.English;
                    }
            }
            if (Base.Config.ForceDefaultPlayerLanguage)
                try
                {
                    TSPlayerB.PlayerLanguage[player.Index] = (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Base.Config.DefaultPlayerLanguage);
                }
                catch (ArgumentException ex)
                {
                    if (Base.Config.debugLevel <= Config.DebugLevel.Info)
                        Logger.doLog("Incorrect DefaultLanguage in Hydra ConfigFile, using default Hydra language", Config.DebugLevel.Info);
                    else
                        Logger.doLog($"Incorrect DefaultLanguage in Hydra ConfigFile, using default Hydra language => Details: {ex}", Config.DebugLevel.Error);
                    TSPlayerB.PlayerLanguage[player.Index] = TSPlayerB.Language.English;
                }
            TSPlayerB.HCountry[player.Index] = player.Country;
        }
        public static string GetUserCountryByIp(string ip)
        {
            IpInfo ipInfo = new IpInfo();
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = ipInfo.Country == "??" ? "N/A" : ipInfo.Country;
            }
            catch (Exception)
            {
                ipInfo.Country = "N/A";
            }
            CachedCountry = ipInfo.Country;
            return ipInfo.Country;
        }
        public class IpInfo
        {
            [JsonProperty("ip")]
            public string Ip { get; set; }

            [JsonProperty("hostname")]
            public string Hostname { get; set; }

            [JsonProperty("city")]
            public string City { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }
        }
    }
}
