using Domain;
using FluentValidation;
using Infrastructura;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Services.Queries
{
    public class GetByIdOrderQuery : IRequest<Order>
    {
        public Guid Id { get; set; }

        public class GetByIdOrderQueryHandler : IRequestHandler<GetByIdOrderQuery, Order?>
        {
            private readonly OrderAppDbContext orderAppDbContext;

            public GetByIdOrderQueryHandler(OrderAppDbContext orderAppDbContext)
            {
                this.orderAppDbContext = orderAppDbContext;
            }

            public async Task<Order?> Handle(GetByIdOrderQuery request, CancellationToken cancellationToken)
            {
                return await orderAppDbContext.Orders.SingleOrDefaultAsync(o => o.Id == request.Id,cancellationToken);
            }
        }
    }
}
