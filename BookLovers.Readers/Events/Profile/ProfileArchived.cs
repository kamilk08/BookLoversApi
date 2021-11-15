using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Profile
{
    public class ProfileArchived : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int Status { get; private set; }

        private ProfileArchived()
        {
        }

        [JsonConstructor]
        protected ProfileArchived(Guid guid, Guid aggregateGuid, int status)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            Status = status;
        }

        public ProfileArchived(Guid aggregateGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            Status = AggregateStatus.Archived.Value;
        }
    }
}