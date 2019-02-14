using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LinkShortener.Models
{
    public class Click
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string LinkId { get; set; }
        public virtual Link Link { get; set; }

    }
}
