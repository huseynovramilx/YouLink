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
            Links = new List<Link>();
        }
        [NotMapped]
        public decimal Money
        {
            get
            {
                
                decimal money = 0;
                foreach (Link link in Links)
                {
                    money += link.Clicks.Count;
                }
                return money;
            }
        }
        [InverseProperty("Owner")]
        public virtual ICollection<Link> Links { get; private set; }
    }
}
