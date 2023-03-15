using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Domain;

namespace infrastructura
{
    public class WarehouseDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Event> events { get; set; } = null!;

        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Cost)
                .HasColumnType("money");

            modelBuilder.Entity<Event>()
                .HasKey(e => new { e.Id, e.EventType });
            modelBuilder.Entity<Event>()
                .Property(e => e.Data).HasJsonConversion();
            modelBuilder.Entity<Event>()
                .Property(e => e.Metadata).HasJsonConversion();

            base.OnModelCreating(modelBuilder);
        }
    }

    public static class ValueConversionExtensions
    {
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
        {
            ValueConverter<T, String> converter = new ValueConverter<T, String>(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<T>(v)!);

            ValueComparer<T> comparer = new ValueComparer<T>(
                (l, r) => JsonConvert.SerializeObject(l) == JsonConvert.SerializeObject(r),
                v => v == null ? 0 : JsonConvert.SerializeObject(v).GetHashCode(),
                v => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(v))!);

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);

            return propertyBuilder;
        }
    }
}
