using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Domain
{
    public sealed class Event
    {
        Event() { }
        public Event(Guid id, string eventType, object data)
        {
            Id = id;
            this.EventType = eventType;
        }

        public Guid Id { get; private set; }

        public string EventType { get; private set; } = string.Empty;

        [Required, Column(Order = 0)]
        public DateTimeOffset DateCreate { get; private set; } = DateTimeOffset.Now;

        [Required]
        public object Data { get; private set; }
    }
}
