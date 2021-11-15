using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Readers
{
    public class ReaderSuspended : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public Guid ProfileGuid { get; private set; }

        public Guid NotificationWallGuid { get; private set; }

        public int ReaderStatus { get; private set; }

        private ReaderSuspended()
        {
        }

        [JsonConstructor]
        protected ReaderSuspended(
            Guid guid,
            Guid aggregateGuid,
            Guid bookcaseGuid,
            Guid profileGuid,
            Guid notificationWallGuid,
            int readerStatus)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            BookcaseGuid = bookcaseGuid;
            ProfileGuid = profileGuid;
            NotificationWallGuid = notificationWallGuid;
            ReaderStatus = readerStatus;
        }

        public ReaderSuspended(Guid readerGuid, Guid profileGuid, Guid notificationWallGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = readerGuid;
            ProfileGuid = profileGuid;
            NotificationWallGuid = notificationWallGuid;
            ReaderStatus = AggregateStatus.Archived.Value;
        }
    }
}