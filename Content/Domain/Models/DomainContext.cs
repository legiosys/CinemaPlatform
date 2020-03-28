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
        public DbSet<Film> Films { get; set; }

        public DomainContext(DbContextOptions<DomainContext> options)
            :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>()
                .HasIndex(f => f.ImdbId)
                .IsUnique();
        }
    }
}
