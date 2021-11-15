using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.ProfileManagers
{
    public class ProfilePrivacyOptionAdded : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int PrivacyTypeId { get; private set; }

        public string PrivacyTypeName { get; private set; }

        public int PrivacyOptionId { get; private set; }

        public string PrivacyOptionName { get; private set; }

        private ProfilePrivacyOptionAdded()
        {
        }

        [JsonConstructor]
        protected ProfilePrivacyOptionAdded(
            Guid guid,
            Guid aggregateGuid,
            int privacyTypeId,
            string privacyTypeName,
            int privacyOptionId,
            string privacyOptionName)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            PrivacyTypeId = privacyTypeId;
            PrivacyTypeName = privacyTypeName;
            PrivacyOptionId = privacyOptionId;
            PrivacyOptionName = privacyOptionName;
        }

        public ProfilePrivacyOptionAdded(
            Guid aggregateGuid,
            int privacyTypeId,
            string privacyTypeName,
            int privacyOptionId,
            string privacyOptionName)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            PrivacyTypeId = privacyTypeId;
            PrivacyTypeName = privacyTypeName;
            PrivacyOptionId = privacyOptionId;
            PrivacyOptionName = privacyOptionName;
        }

        public static ProfilePrivacyOptionAdded Initialize() =>
            new ProfilePrivacyOptionAdded();

        public ProfilePrivacyOptionAdded WithAggregate(Guid aggregateGuid) =>
            new ProfilePrivacyOptionAdded(aggregateGuid, PrivacyTypeId, PrivacyTypeName, PrivacyOptionId,
                PrivacyOptionName);

        public ProfilePrivacyOptionAdded WithPrivacyType(
            int privacyTypeId,
            string privacyTypeName)
        {
            return new ProfilePrivacyOptionAdded(AggregateGuid, privacyTypeId, privacyTypeName, PrivacyOptionId,
                PrivacyOptionName);
        }

        public ProfilePrivacyOptionAdded WithOption(
            int optionId,
            string optionPrivacyName)
        {
            return new ProfilePrivacyOptionAdded(AggregateGuid, PrivacyTypeId, PrivacyTypeName, optionId,
                optionPrivacyName);
        }
    }
}