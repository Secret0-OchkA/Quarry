using Domain;
using infrastructura;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Warehouse.Services.Commands
{
    public class ImportProductCommand : ICommand<Product?>
    {
        public Guid ProductId { get; set; }

        public double Delta { get; set; }

        public class ImportProductCommandHandler : IRequestHandler<ImportProductCommand, Product?>
        {
            private readonly WarehouseDbContext context;

            public ImportProductCommandHandler(WarehouseDbContext context)
            {
                this.context = context;
            }

            public async Task<Product?> Handle(ImportProductCommand request, CancellationToken cancellationToken)
            {
                Product? product = await context.Products
                    .AsTracking()
                    .SingleOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken: cancellationToken);

                if (product == null) return null;

                product.Count += request.Delta;

                context.Update(product);
                await context.SaveChangesAsync(cancellationToken);

                return product;
            }
        }
    }
}
