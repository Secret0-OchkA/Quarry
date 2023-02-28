using Domain;
using FluentValidation;
using infrastructura;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Commands
{
    public class UpdateProductCommand : IRequest<Product?>
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public float Count { get; set; }
        public string Unit { get; set; } = string.Empty;

        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product?>
        {
            private readonly WarehouseDbContext context;

            public UpdateProductCommandHandler(WarehouseDbContext context)
            {
                this.context = context;
            }

            public async Task<Product?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                Product? product = await context.Products
                    .AsTracking()
                    .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

                if (product == null) return null;

                product.Description = request.Description;
                product.Cost = request.Cost;
                product.Count = request.Count;
                product.Unit = request.Unit;
                product.Name = request.Name;

                context.Update(product);
                await context.SaveChangesAsync(cancellationToken);
                return product;
            }
        }

        public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
        {
            public UpdateProductCommandValidator()
            {
                RuleFor(p => p.Name).NotEmpty().WithMessage("Empty name");
                RuleFor(p => p.Unit).NotEmpty().WithMessage("empty unit");
                RuleFor(p => p.Cost).GreaterThan(0).WithMessage("cost less 0");
                RuleFor(p => p.Count).GreaterThan(-1).WithMessage("count less 0");
            }
        }
    }
}
