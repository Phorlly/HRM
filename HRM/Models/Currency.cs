using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRM.Models
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)] 
        public string Name { get; set; }

        [MaxLength(50)]
        public string Code { get; set; } 

        [MaxLength(10)]
        public string Symbol { get; set; } 

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; } 
        public bool Status { get; set; } 
        
    }
}