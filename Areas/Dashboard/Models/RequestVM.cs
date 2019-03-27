using System.ComponentModel.DataAnnotations;
using LinkShortener.Models;

namespace LinkShortener.Areas.Dashboard.Models
{
    public class RequestVM
    {
        [Display(Name = "Recipient type")]
        public int RecipientTypeID{get; set;}

        [Display(Name = "Receiver")]
        public string Receiver{get ;set;}
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        public decimal Money{get; set;}
    }
}