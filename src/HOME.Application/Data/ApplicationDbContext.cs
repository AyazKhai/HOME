using HOME.DOMAIN.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HOME.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация сущностей
            modelBuilder.Entity<Equipment>()
                .Property(e => e.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Equipment>()
                .Property(e => e.Status)
                .HasConversion<string>();
        }

        public DbSet<Equipment> Equipments { get; set; }
    }
}
