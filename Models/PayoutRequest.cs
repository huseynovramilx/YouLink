using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkShortener.Models
{
    public class PayoutRequest
    {
        public int ID{get; set;}
        [DataType(DataType.Currency)]
        public decimal Money{get; set;}

        public string OwnerId{get ;set;}
        [ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner{get; set;}

        public bool Paid{get;set;}
    }
}