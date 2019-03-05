using System.ComponentModel.DataAnnotations;
using LinkShortener.Models;

namespace LinkShortener.Areas.Dashboard.Models
{
    public class RequestVM
    {
        public int RecipientTypeID{get; set;}

        public string Receiver{get ;set;}
        [DataType(DataType.Currency)]
        public decimal Money{get; set;}
    }
}