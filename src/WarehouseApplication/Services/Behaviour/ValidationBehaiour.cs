using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Services.Behaviour
{
    public class ValidationBehaiour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;
        private readonly ILogger<ValidationBehaiour<TRequest, TResponse>> logger;

        public ValidationBehaiour(IEnumerable<IValidator<TRequest>> validators,
            ILogger<ValidationBehaiour<TRequest, TResponse>> logger)
        {
            this.validators = validators;
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any()) return await next();

            string typeName = request.GetType().Name;

            logger.LogInformation("Validate command {typeName}", typeName);

            ValidationContext<TRequest> context = new(request);
            ValidationResult[] validationResults =
                await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            List<ValidationFailure> failures = validationResults.SelectMany(res => res.Errors)
                .Where(error => error != null).ToList();

            if (!failures.Any()) return await next();

            logger.LogWarning(
                "Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}",
                typeName, request, failures);

            throw new FluentValidation.ValidationException(failures);
        }
    }
}
