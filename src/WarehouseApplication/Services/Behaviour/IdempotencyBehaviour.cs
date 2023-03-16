using infrastructura;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
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
        private readonly IIdempotencyKeyProvider keyProvider;
        private readonly IEventManager eventManager;

        public IdempotencyBehaviour(
            ILogger<IdempotencyBehaviour<TRequest, TResponse>> logger,
            IIdempotencyKeyProvider keyProvider,
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
            {
                logger.LogInformation("{Property}, : complite {@Value}", command.GetType().Name, command);
                throw new FluentValidation.ValidationException("command should have Idempotency-Key header");
            }

            var eventRecord = await eventManager.Get(idempotencyKey, command.GetType().Name);
            if (eventRecord != null)
            {
                logger.LogInformation("{Property}, : complite {@Value}", command.GetType().Name, command);
                throw new FluentValidation.ValidationException("command complite");
            }
            

            await eventManager.Save(idempotencyKey,command.GetType().Name,new JObject(command), new JObject());
            return await next();
        }
    }
}
