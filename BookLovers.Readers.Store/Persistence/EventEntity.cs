using System;
using BookLovers.Base.Infrastructure.Persistence;

namespace BookLovers.Readers.Store.Persistence
{
    public class EventEntity : IEventEntity
    {
        public int Id { get; set; }

        public Guid AggregateGuid { get; set; }

        public string Data { get; set; }

        public string Type { get; set; }

        public string Assembly { get; set; }

        public int Version { get; set; }

        protected EventEntity()
        {
        }

        public EventEntity(Guid aggregateGuid, string data, string type, string assembly)
        {
            AggregateGuid = aggregateGuid;
            Data = data;
            Type = type;
            Assembly = assembly;
        }
    }
}