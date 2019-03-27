using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkShortener.Models
{
    public class AppOptions
    {
        public AppOptions()
        {

        }
        public decimal MoneyPerClick { get; set; }
        public int ReferralPercentage { get; set; }
        public string RecaptchaSiteKey { get; set; }
    }
}
