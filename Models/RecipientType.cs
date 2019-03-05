using System.ComponentModel.DataAnnotations;

namespace LinkShortener.Models
{
    public class RecipientType
    {
        [Key]
        public int ID{get; set;}
        [Required]
        public string Method { get; set; }
        [Required]
        public string Name{get;set;}
    }
}