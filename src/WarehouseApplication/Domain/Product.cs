using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public sealed class Product
    {
        protected Product() { }
        public Product(
            string Name, string Description, decimal Cost,
            double Count, string Unit) {
            this.Id = Guid.NewGuid();
            this.Name = Name;
            this.Description = Description;
            this.Cost = Cost;
            this.Count = Count;
            this.Unit = Unit;
        }

        public Guid Id { get; private set; }
        [Required, MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public double Count { get; set; }
        [Required]
        public string Unit { get; set; } = string.Empty;

        public string? Scope { get; private set; } = Environment.GetEnvironmentVariable("SCOPE");
    }
}
