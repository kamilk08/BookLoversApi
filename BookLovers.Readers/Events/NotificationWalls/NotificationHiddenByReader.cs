using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.NotificationWalls
{
    public class NotificationHiddenByReader : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid NotificationGuid { get; private set; }

        private NotificationHiddenByReader()
        {
        }

        [JsonConstructor]
        protected NotificationHiddenByReader(Guid guid, Guid aggregateGuid, Guid notificationGuid)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            NotificationGuid = notificationGuid;
        }

        public NotificationHiddenByReader(Guid aggregateGuid, Guid notificationGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            NotificationGuid = notificationGuid;
        }
    }
}