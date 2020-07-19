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
                TSPlayerB.PlayerLanguage[player.Index] = (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Config.DefaultLanguage);
                player.Country = "Localhost";
            }
            else
            {
                if (player.Country == "Brasil" || player.Country == "Brazil" || player.Country == "Portugal" || player.Country == "Angola" || player.Country == "Moçambique" || player.Country == "Guiné-Bissau" || player.Country == "Cabo Verde" || player.Country == "São Tomé e Príncipe" || player.Country == "Timor Leste" || player.Country == "BR" || player.Country == "PT" || player.Country == "AO" || player.Country == "MZ" || player.Country == "GW" || player.Country == "ST" || player.Country == "CV" || player.Country == "TL")
                    TSPlayerB.PlayerLanguage[player.Index] = TSPlayerB.Language.Portuguese;
                else if (player.Country == "United States" || player.Country == "US")
                    TSPlayerB.PlayerLanguage[player.Index] = TSPlayerB.Language.English;
                else if (player.Country == "Argentina" || player.Country == "AR")
                    TSPlayerB.PlayerLanguage[player.Index] = TSPlayerB.Language.Spanish;
                else
                    try
                    {
                        TSPlayerB.PlayerLanguage[player.Index] = (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Config.DefaultLanguage);
                    }
                    catch (ArgumentException ex)
                    {
                        //Log Incorrect Config DefaultLanguage
                        TSPlayerB.PlayerLanguage[player.Index] = TSPlayerB.Language.English;
                    }
            }
            if (Config.ForceDefaultLanguage)
                try
                {
                    TSPlayerB.PlayerLanguage[player.Index] = (TSPlayerB.Language)Enum.Parse(typeof(TSPlayerB.Language), Config.DefaultLanguage);
                }
                catch (ArgumentException ex)
                {
                    //Log Incorrect Config DefaultLanguage
                    TSPlayerB.PlayerLanguage[player.Index] = TSPlayerB.Language.English;
                }
            TSPlayerB.HCountry[player.Index] = player.Country;
            Console.WriteLine($"HCountry => {TSPlayerB.HCountry[player.Index]}");
        }
        public static string GetUserCountryByIp(string ip)
        {
            IpInfo ipInfo = new IpInfo();
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                //ipInfo.Country = myRI1.EnglishName;
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
