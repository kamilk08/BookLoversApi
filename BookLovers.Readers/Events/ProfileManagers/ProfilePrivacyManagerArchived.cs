using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.ProfileManagers
{
    public class ProfilePrivacyManagerArchived : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int Status { get; private set; }

        private ProfilePrivacyManagerArchived()
        {
        }

        [JsonConstructor]
        protected ProfilePrivacyManagerArchived(Guid guid, Guid aggregateGuid, int status)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            Status = status;
        }

        public ProfilePrivacyManagerArchived(Guid aggregateGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            Status = AggregateStatus.Active.Value;
        }
    }
}