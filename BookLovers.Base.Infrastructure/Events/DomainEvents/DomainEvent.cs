using System;

namespace BookLovers.Base.Infrastructure.Events.DomainEvents
{
    public class DomainEvent
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public DateTime OccuredAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public string Data { get; set; }

        public string Type { get; set; }

        public string Assembly { get; set; }

        private DomainEvent()
        {
        }

        public DomainEvent(Guid guid, string data, string type, string assembly)
        {
            Guid = guid;
            OccuredAt = DateTime.UtcNow;
            Data = data;
            Type = type;
            Assembly = assembly;
        }
    }
}