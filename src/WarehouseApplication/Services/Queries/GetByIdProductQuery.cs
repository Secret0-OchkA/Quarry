using Domain;
using infrastructura;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Services.Queries
{
    public class GetByIdProductQuery : IQuery<Product>
    {
        public Guid Id { get; set; }

        public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQuery, Product?>
        {
            private readonly WarehouseDbContext context;

            public GetByIdProductQueryHandler(WarehouseDbContext context)
            {
                this.context = context;
            }

            public async Task<Product?> Handle(GetByIdProductQuery request, CancellationToken cancellationToken)
            {
                return await context.Products.SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            }
        }
    }
}
