using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using LinkShortener.Data;

namespace LinkShortener.Models
{
    public class ApplicationUser:IdentityUser
    {
        public ApplicationUser()
        {
           
        }
        public string ReferrerId { get; set; }

        [ForeignKey("ReferrerId")]
        public virtual ApplicationUser Referrer { get; private set; }

        [InverseProperty("Referrer")]
        public virtual ICollection<ApplicationUser> Referrals { get; private set; }

        [InverseProperty("Owner")]
        public virtual ICollection<Link> Links { get; private set; }

        [InverseProperty("Owner")]
        public RecipientSettings DefaultRecipientSettings{get; set;}

        [Required]
        public Currency Currency { get; set; }

        public string Receiver{get ;set;}
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(14,2)")]
        public decimal EarnedMoney { get; set; }

        [Column(TypeName = "decimal(14,2)")]
        public decimal RequestedMoney { get; set; }

        [Column(TypeName = "decimal(14,2)")]
        public decimal ReferralMoney { get; set; }

    }
}
