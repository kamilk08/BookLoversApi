using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.ProfileManagers
{
    public class ProfilePrivacyManagerCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ProfileGuid { get; private set; }

        public int Status { get; private set; }

        private ProfilePrivacyManagerCreated()
        {
        }

        [JsonConstructor]
        protected ProfilePrivacyManagerCreated(
            Guid guid,
            Guid aggregateGuid,
            Guid profileGuid,
            int status)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ProfileGuid = profileGuid;
            Status = status;
        }

        public ProfilePrivacyManagerCreated(Guid aggregateGuid, Guid profileGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ProfileGuid = profileGuid;
            Status = AggregateStatus.Active.Value;
        }
    }
}