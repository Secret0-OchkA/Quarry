using Domain;
using Infrastructura;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Features.Queries
{
    public class GetAllProductQuery : IRequest<IEnumerable<Product>>
    {

        public class GetAllProductQuerryHandler : IRequestHandler<GetAllProductQuery, IEnumerable<Product>>
        {
            private readonly IRepository<Product> productRepository;

            public GetAllProductQuerryHandler(IRepository<Product> productRepository)
            {
                this.productRepository = productRepository;
            }

            public async Task<IEnumerable<Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
            {
                return await productRepository.GetAll();
            }
        }
    }
}
