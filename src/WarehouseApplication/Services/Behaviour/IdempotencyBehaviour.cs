using infrastructura;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Services.Commands;
using Warehouse.Services.Idempotency;

namespace Warehouse.Services.Behaviour
{
    public class IdempotencyBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<IdempotencyBehaviour<TRequest, TResponse>> logger;
        private readonly IdempotencyKeyProvider keyProvider;
        private readonly IEventManager eventManager;

        public IdempotencyBehaviour(
            ILogger<IdempotencyBehaviour<TRequest,TResponse>> logger,
            IdempotencyKeyProvider keyProvider,
            IEventManager eventRecordManager)
        {
            this.logger = logger;
            this.keyProvider = keyProvider;
            this.eventManager = eventRecordManager;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if(request is not ICommand<TResponse> command)
                return await next();

            if(!Guid.TryParse(await keyProvider.Get(),out Guid idempotencyKey))
                return await next();

            var eventRecord = await eventManager.Get(idempotencyKey);
            if (eventRecord != null)
            {
                logger.LogInformation("{Property}, : complite {@Value}", command.GetType().Name, command);
                throw new FluentValidation.ValidationException("command complite");
            }


            await eventManager.Save(idempotencyKey,command.GetType().Name,command);
            return await next();
        }
    }
}
