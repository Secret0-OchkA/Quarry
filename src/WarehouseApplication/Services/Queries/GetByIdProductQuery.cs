using Domain;
using Infrastructura;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Services.Queries
{
    public class GetByIdProductQuery : IRequest<Product>
    {
        public Guid Id { get; set; }

        public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQuery, Product?>
        {
            private readonly IRepository<Product> productRepoisitory;

            public GetByIdProductQueryHandler(IRepository<Product> productRepoisitory)
            {
                this.productRepoisitory = productRepoisitory;
            }

            public async Task<Product?> Handle(GetByIdProductQuery request, CancellationToken cancellationToken)
            {
                return await productRepoisitory.GetById(request.Id);
            }
        }
    }
}
