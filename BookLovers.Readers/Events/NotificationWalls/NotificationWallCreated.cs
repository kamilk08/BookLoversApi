using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.NotificationWalls
{
    public class NotificationWallCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int Status { get; private set; }

        private NotificationWallCreated()
        {
        }

        [JsonConstructor]
        protected NotificationWallCreated(Guid guid, Guid aggregateGuid, Guid readerGuid, byte status)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            Status = status;
        }

        public NotificationWallCreated(Guid aggregateGuid, Guid readerGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            Status = AggregateStatus.Active.Value;
        }
    }
}