using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.NotificationWalls
{
    public class NotificationSeenByReader : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid NotificationGuid { get; private set; }

        public DateTime SeenAt { get; private set; }

        private NotificationSeenByReader()
        {
        }

        [JsonConstructor]
        protected NotificationSeenByReader(
            Guid guid,
            Guid aggregateGuid,
            Guid notificationGuid,
            DateTime seenAt)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            NotificationGuid = notificationGuid;
            SeenAt = seenAt;
        }

        public NotificationSeenByReader(Guid aggregateGuid, Guid notificationGuid, DateTime seenAt)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            NotificationGuid = notificationGuid;
            SeenAt = seenAt;
        }
    }
}