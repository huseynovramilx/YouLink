using LinkShortener.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LinkShortener.Models
{
    public class Link
    { 
        public Link()
        {
            Clicks = new List<Click>();
            isReferral = false;
        }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }

        [Key]
        public string Id { get; set; }
        
        public virtual ICollection<Click> Clicks { get; set; }

        [Required]
        [Url]
        public string FullUrl { get; set; }

 
        public string OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }

        public bool isReferral{get; set;}
    }


    public class ReferralLink:Link{
        public ReferralLink(){
            Clicks = new List<Click>();
            isReferral = true;
        }
    }
}
