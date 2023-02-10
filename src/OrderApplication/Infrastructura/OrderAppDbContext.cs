
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructura
{
    public interface IOrderAppDbContext
    {
        DbSet<Order> Orders { get; }

        Task<int> SaveChanges();
    }

    public class OrderAppDbContext : DbContext, IOrderAppDbContext
    {
        public DbSet<Order> Orders { get; protected set; } = null!;

        public OrderAppDbContext(DbContextOptions<OrderAppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChanges()
            => await base.SaveChangesAsync();
    }
}
