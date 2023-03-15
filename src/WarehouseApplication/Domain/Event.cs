using Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Warehouse.Domain
{
    public sealed class Event
    {
        private Event() { }
        public Event(Guid id, string eventType, object data)
        {
            Id = id;
            this.EventType = eventType;
            if (data is string)
                Data = data as string;
            else
                Data = JsonConvert.SerializeObject(data);
        }

        public Guid Id { get; private set; }

        public string EventType { get; private set; } = string.Empty;

        [Required, Column(Order = 0)]
        public DateTimeOffset DateCreate { get; private set; } = DateTimeOffset.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Version { get; private set; }

        [Required]
        public string Data { get; private set; }

        [Required]
        public object Metadata { get; private set; } = Environment.GetEnvironmentVariable("SCOPE") ?? new object();
    }
}
