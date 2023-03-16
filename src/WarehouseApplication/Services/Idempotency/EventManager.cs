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

        public EventManager(WarehouseDbContext context)
        {
            this.context = context;
        }

        public async Task<Event?> Get(Guid id, string type)
            => await context.events.SingleOrDefaultAsync(e => e.Id == id && e.EventType.Equals(type));

        public async Task Save(Guid eventId, string eventName, JObject data, JObject metadata)
        {
            var newEvent = new Event(eventId, eventName, data, metadata);

            await context.AddAsync(newEvent);
            await context.SaveChangesAsync();
        }
    }
}
