using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.ProfileManagers
{
    public class ProfilePrivacyOptionChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int PrivacyTypeId { get; private set; }

        public int PrivacyOptionId { get; private set; }

        private ProfilePrivacyOptionChanged()
        {
        }

        [JsonConstructor]
        protected ProfilePrivacyOptionChanged(
            Guid guid,
            Guid aggregateGuid,
            int privacyTypeId,
            int privacyOptionId)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            PrivacyTypeId = privacyTypeId;
            PrivacyOptionId = privacyOptionId;
        }

        public ProfilePrivacyOptionChanged(Guid aggregateGuid, int privacyTypeId, int privacyOptionId)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            PrivacyTypeId = privacyTypeId;
            PrivacyOptionId = privacyOptionId;
        }
    }
}