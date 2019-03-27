using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkShortener.Models
{
    public class PayoutRequest
    {
        public int ID{get; set;}
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        public decimal Money{get; set;}

        public string OwnerId{get ;set;}
        [ForeignKey("OwnerId")]
        [Display(Name = "Owner")]
        public virtual ApplicationUser Owner{get; set;}
        [Display(Name = "Paid")]
        public bool Paid{get;set;}
    }
}