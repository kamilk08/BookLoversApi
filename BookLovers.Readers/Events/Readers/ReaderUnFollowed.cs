using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Readers
{
    public class ReaderUnFollowed : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid NotificationWallGuid { get; private set; }

        public Guid UnFollowedByGuid { get; private set; }

        private ReaderUnFollowed()
        {
        }

        [JsonConstructor]
        protected ReaderUnFollowed(
            Guid guid,
            Guid aggregateGuid,
            Guid notificationWallGuid,
            Guid followedByGuid)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            NotificationWallGuid = notificationWallGuid;
            UnFollowedByGuid = followedByGuid;
        }

        public ReaderUnFollowed(Guid aggregateGuid, Guid notificationWallGuid,
            Guid followedByGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            NotificationWallGuid = notificationWallGuid;
            UnFollowedByGuid = followedByGuid;
        }
    }
}