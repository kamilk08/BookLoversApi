using System;
using BookLovers.Base.Infrastructure.Persistence;

namespace BookLovers.Publication.Store.Persistence
{
    public class EventEntity : IEventEntity
    {
        public int Id { get; set; }

        public Guid AggregateGuid { get; set; }

        public string Data { get; set; }

        public string Type { get; set; }

        public string Assembly { get; set; }

        public int Version { get; set; }

        public EventEntity()
        {
        }

        public EventEntity(Guid aggregateGuid, string data, string type, string assembly)
        {
            this.AggregateGuid = aggregateGuid;
            this.Data = data;
            this.Type = type;
            this.Assembly = assembly;
        }
    }
}