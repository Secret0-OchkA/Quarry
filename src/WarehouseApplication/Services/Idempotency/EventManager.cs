using Domain;
using infrastructura;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Domain;
using Warehouse.Services.Commands;
using Warehouse.Services.RabbitMq;

namespace Warehouse.Services.Idempotency
{
    public interface IEventManager
    {
        Task<Event?> Get(Guid id, string type);
        Task Save(Guid eventId, string eventName, JObject data, JObject metadata);
    }

    public class EventManager : IEventManager
    {
        private readonly WarehouseDbContext context;
        private readonly IRabbitMQProducer producer;

        public EventManager(WarehouseDbContext context, IRabbitMQProducer producer)
        {
            this.context = context;
            this.producer = producer;
        }

        public async Task<Event?> Get(Guid id, string type)
            => await context.events.SingleOrDefaultAsync(e => e.Id == id && e.EventType.Equals(type));

        public async Task Save(Guid eventId, string eventName, JObject data, JObject metadata)
        {
            var newEvent = new Event(eventId, eventName, data, metadata);

            await context.AddAsync(newEvent);
            await context.SaveChangesAsync();

            producer.SendMessage(newEvent);
        }
    }
}
