using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRM.DTOs
{
    public class UserDto
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Username { get; set; }

        public bool Gender { get; set; }

        [StringLength(52)]
        public string Email { get; set; }

        [StringLength(512)]
        public string Address { get; set; }

        [StringLength(12)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Photo { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool IsAdmin { get; set; }
        public bool Status { get; set; }
    }
}