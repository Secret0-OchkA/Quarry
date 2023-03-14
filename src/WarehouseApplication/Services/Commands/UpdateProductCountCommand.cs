using Domain;
using infrastructura;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Warehouse.Services.Commands
{
    public class UpdateProductCountCommand : ICommand<Unit>
    {
        public Guid ProductId { get; set; }

        public double Delta { get; set; }

        public class UpdateProductCountCommandHandler : IRequestHandler<UpdateProductCountCommand>
        {
            private readonly WarehouseDbContext context;

            public UpdateProductCountCommandHandler(WarehouseDbContext context)
            {
                this.context = context;
            }

            public Task<Unit> Handle(UpdateProductCountCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
