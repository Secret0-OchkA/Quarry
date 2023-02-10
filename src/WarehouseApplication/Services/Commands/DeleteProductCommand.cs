using Domain;
using FluentValidation;
using Infrastructura;
using MediatR;
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
            private readonly IRepository<Product> productRepository;

            public DeleteProductCommandHandler(IRepository<Product> productRepository)
            {
                this.productRepository = productRepository;
            }

            public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                return await productRepository.Delete(request.id);
            }
        }

        public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
        {
            public DeleteProductCommandValidator() { }
        }
    }
}
