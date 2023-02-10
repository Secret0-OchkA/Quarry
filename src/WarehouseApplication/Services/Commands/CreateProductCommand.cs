using Domain;
using FluentValidation;
using Infrastructura;
using MediatR;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commands
{
    public class CreateProductCommand : IRequest<Product>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public float Count { get; set; }
        public string Unit { get; set; } = string.Empty;

        public class AddProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
        {
            private readonly IRepository<Product> productRepository;

            public AddProductCommandHandler(IRepository<Product> productRepository)
            {
                this.productRepository = productRepository;
            }

            public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                Product product = new Product(
                    Name: request.Name,
                    Description: request.Description,
                    Cost: request.Cost,
                    Count: request.Count,
                    Unit: request.Unit
                );

                await productRepository.Cereate(product);

                return product;
            }
        }

        public class AddProductCommandValidator : AbstractValidator<CreateProductCommand>
        {
            public AddProductCommandValidator()
            {
                RuleFor(p => p.Name).NotEmpty().WithMessage("Empty name");
                RuleFor(p => p.Unit).NotEmpty().WithMessage("empty unit");
                RuleFor(p => p.Cost).GreaterThan(0).WithMessage("cost less 0");
                RuleFor(p => p.Count).GreaterThan(-1).WithMessage("count less 0");
            }
        }
    }
}
