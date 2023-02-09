using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Product : IEntity
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
        public Guid Id { get; protected set; }
        public string Name { get; protected set; } = string.Empty;
        public string Description { get; protected set; } = string.Empty;
        public decimal Cost { get; protected set; }
        public double Count { get; protected set; }
        public string Unit { get; protected set; } = string.Empty;
        public string Owner { get; protected set; } = Environment.GetEnvironmentVariable("OWNER") ?? string.Empty;
    }
}
