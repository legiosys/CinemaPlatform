using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class DomainContext : DbContext
    {
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DomainContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=domaindb;Trusted_Connection=True;");
        }
    }
}
