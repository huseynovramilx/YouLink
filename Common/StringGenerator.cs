using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShortener.Common
{
    public class StringGenerator
    {
        public static string availableChars = "0123456789QWERTYUIOPASDFGHJKLZXCVBNM";
        public static string getString(int num)
        {
            StringBuilder result = new StringBuilder();
            int len = availableChars.Length;
            while (num > 0)
            {
                int rem = num % len;
                num /= len;
                result.Append(availableChars[rem]);
            }
            return result.ToString();
        }
    }
}
