using Domain;
using infrastructura;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Services.Queries
{
    public class GetAllProductQuery : IQuery<IEnumerable<Product>>
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 10;

        public class GetAllProductQuerryHandler : IRequestHandler<GetAllProductQuery, IEnumerable<Product>>
        {
            private readonly WarehouseDbContext context;

            public GetAllProductQuerryHandler(WarehouseDbContext context)
            {
                this.context = context;
            }

            public async Task<IEnumerable<Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
            {
                return context.Products.Skip(request.Page * request.PageSize).Take(request.PageSize);
            }
        }
    }
}
