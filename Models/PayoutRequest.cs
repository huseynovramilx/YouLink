using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkShortener.Models
{
    public class PayoutRequest
    {
        public int ID{get; set;}
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(14,2)")]
        public decimal Money{get; set;}
        [Required]
        public RecipientSettings RecipientSettings { get; set; }
        [Display(Name = "Paid")]
        public bool Paid{get;set;}
    }
}