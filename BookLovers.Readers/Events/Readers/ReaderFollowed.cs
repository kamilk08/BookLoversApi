using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Readers
{
    public class ReaderFollowed : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid NotificationWallGuid { get; private set; }

        public Guid FollowedByGuid { get; private set; }

        private ReaderFollowed()
        {
        }

        [JsonConstructor]
        protected ReaderFollowed(
            Guid guid,
            Guid aggregateGuid,
            Guid notificationWallGuid,
            Guid followedByGuid)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            NotificationWallGuid = notificationWallGuid;
            FollowedByGuid = followedByGuid;
        }

        public ReaderFollowed(Guid aggregateGuid, Guid notificationWallGuid, Guid followedByGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            NotificationWallGuid = notificationWallGuid;
            FollowedByGuid = followedByGuid;
        }
    }
}