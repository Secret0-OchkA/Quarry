using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;
using MediatR;
using Newtonsoft.Json;
using Warehouse.Domain;
using System.Reflection;

namespace Warehouse.Services.RabbitMq
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly ILogger<RabbitMqConsumer> logger;
        private readonly IMediator mediator;

        public RabbitMqConsumer(IConnectionFactory factory, ILogger<RabbitMqConsumer> logger, IMediator mediator)
        {
            this.factory = factory;
            this.logger = logger;
            this.mediator = mediator;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            
            cancellationToken.ThrowIfCancellationRequested();
            channel.QueueDeclare(queue: "MyQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                logger.LogInformation($"get msg: {content}");

                var eventOjb = JsonConvert.DeserializeObject<Event>(content);
                Type eventType = Type.GetType("Warehouse.Services.Commands." + eventOjb.EventType);
                var command = JsonConvert.DeserializeObject(eventOjb.Data, eventType);

                await mediator.Send(command);

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume("MyQueue", false, consumer);

            return Task.CompletedTask;
        }
    }
}
