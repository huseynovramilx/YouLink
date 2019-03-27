using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkShortener.Models
{
    public class AuthMessageSenderOptions
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int SmtpServerPort { get; set; }

        public string SmtpServerHost { get; set; }
    }
}
