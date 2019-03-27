using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkShortener.Services
{
    public class RecaptchaOptions
    {
        public string Url { get; set; }
        public string SecretKey { get; set; }
    }
}
