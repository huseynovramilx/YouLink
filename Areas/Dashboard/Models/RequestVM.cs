using System.ComponentModel.DataAnnotations;
using LinkShortener.Models;

namespace LinkShortener.Areas.Dashboard.Models
{
    public class RequestVM
    {
        [Display(Name = "Recipient type")]
        [Required(ErrorMessage = "Recipient type is required")]
        public int RecipientTypeID{get; set;}

        [Display(Name = "Receiver")]
        [Required(ErrorMessage = "Recipient type is required")]
        public string Receiver{get ;set;}
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        [Range(10, 1000, ErrorMessage = "Requested {0} should be greater than or equal to {1}")]
        public decimal Money{get; set;}
    }
}