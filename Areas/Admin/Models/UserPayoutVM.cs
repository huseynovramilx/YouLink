using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LinkShortener.Areas.Admin.Models
{
    public class UserPayoutVM
    {

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Currency)]
        public decimal Payout { get; set; }
    }
}
