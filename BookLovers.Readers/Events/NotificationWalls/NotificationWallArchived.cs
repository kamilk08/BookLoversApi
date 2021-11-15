using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.NotificationWalls
{
    public class NotificationWallArchived : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int Status { get; private set; }

        private NotificationWallArchived()
        {
        }

        [JsonConstructor]
        protected NotificationWallArchived(Guid guid, Guid aggregateGuid, int status)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            Status = status;
        }

        public NotificationWallArchived(Guid aggregateGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            Status = AggregateStatus.Archived.Value;
        }
    }
}