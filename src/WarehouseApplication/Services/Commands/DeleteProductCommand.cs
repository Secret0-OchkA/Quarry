using Domain;
using FluentValidation;
using infrastructura;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commands
{
    public class DeleteProductCommand : IRequest<int>
    {
        public Guid id { get; set; }

        public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
        {
            private readonly WarehouseDbContext context;

            public DeleteProductCommandHandler(WarehouseDbContext context)
            {
                this.context = context;
            }

            public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                Product? product = await context.Products
                    .SingleOrDefaultAsync(p => p.Id == request.id,cancellationToken);

                if (product == null) return 0;

                context.Remove(product);
                return await context.SaveChangesAsync(cancellationToken);
            }
        }

        public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
        {
            public DeleteProductCommandValidator() { }
        }
    }
}
