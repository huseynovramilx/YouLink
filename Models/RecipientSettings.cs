using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkShortener.Models
{
    public class RecipientSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID{get; set;}
        [Required]
        public string Receiver { get; set; }
        [Required]
        public RecipientType RecipientType { get; set; }
        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        [InverseProperty("DefaultRecipientSettings")]
        public virtual ApplicationUser Owner { get; set; }
    }
}