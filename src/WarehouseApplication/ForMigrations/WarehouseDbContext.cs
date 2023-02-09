using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForMigrations
{
    public class WarehouseDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;

        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Cost)
                .HasColumnType("money");

            base.OnModelCreating(modelBuilder);
        }
    }
}
