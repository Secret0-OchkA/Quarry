
using Domain;
using Infrastructura;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Queries
{
    public class GetAllOrderQuery : IRequest<IEnumerable<Order>>
    {
        public int page { get; set; } = 0;
        public int pageSize { get; set; } = 10;

        public class GetAllOrderQuerryHandler : IRequestHandler<GetAllOrderQuery, IEnumerable<Order>>
        {
            private readonly OrderAppDbContext orderAppDbContext;

            public GetAllOrderQuerryHandler(OrderAppDbContext orderAppDbContext)
            {
                this.orderAppDbContext = orderAppDbContext;
            }

            public async Task<IEnumerable<Order>> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
            {
                return await orderAppDbContext.Orders.Skip(request.page * request.pageSize).Take(request.pageSize).ToListAsync(cancellationToken);
            }
        }
    }
}
