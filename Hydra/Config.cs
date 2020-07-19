using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Hydra
{
    public class Config
    {
        public bool isMobileServer = Main.maxPlayers == 16 ? true : false;
        public static bool MultiLanguageSupport = true;
        public static string DefaultLanguage = "English";
        public static bool ForceDefaultLanguage = false;
    }
}
