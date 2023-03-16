using Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public Event(Guid id, string eventType, JObject Data, JObject Metadata)
        {
            Id = id;
            this.EventType = eventType;
            this.Data = Data;
            this.Metadata = Metadata;
        }

        public Guid Id { get; private set; }

        public string EventType { get; private set; } = string.Empty;

        [Column(Order = 0)]
        public DateTimeOffset DateCreate { get; private set; } = DateTimeOffset.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Version { get; private set; }

        public JObject Data { get; private set; }

        public JObject Metadata { get; private set; } //= Environment.GetEnvironmentVariable("SCOPE") ?? new object();
    }
}
