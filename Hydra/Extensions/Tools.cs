using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Extensions
{
    public class Tools
    {
        public static string BlankLine(int number)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < number; i++)
            {
                sb.Append("\r\n");
            }

            return sb.ToString();
        }
    }
}
