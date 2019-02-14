using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkShortener.Models
{

    public class PayoutBatch
    {

        public PayoutBatch()
        {
            Payouts = new List<Payout>();
            ID = DateTime.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public DateTime ID { get; set; }
        public string PayoutPaypalBatchId { get; set; }

        public string Status { get; set; }
        [Required]
        public string EmailMessage { get; set; }
        [Required]
        public string EmailSubject { get; set; }
        public ICollection<Payout> Payouts { get; private set; }

        public static int GetTodayID()
        {
            return Convert.ToInt32(DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString());
        }
    }

    public class Payout
    {
        [Required]
        public string RecipientType { get; set; }
        public string Note { get; set; }

        [Key]
        public int ID { get; set; }
        [Required]
        public string Receiver { get; set; }

        [ForeignKey("PayoutBatch")]
        public DateTime PayoutBatchId { get; set; }
        public virtual PayoutBatch PayoutBatch { get; set; }
        [Required]
        [Column(TypeName="decimal(18,2)")]
        public decimal Money { get; set; }
        [Required]
        public string Currency { get; set; }
    }

}