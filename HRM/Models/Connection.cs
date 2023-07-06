using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HRM.Models
{
    public class ApplicationDbContext : DbContext
    {
        //Register Table
        public DbSet<User> Users { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        //public DbSet<Attendance> Attendances { get; set; } 

        //Constructor Get Connection
        public ApplicationDbContext() : base("DefaultConnection") { } 
    }
}