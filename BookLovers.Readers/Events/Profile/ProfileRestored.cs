using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Profile
{
    public class ProfileRestored : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int SocialProfileStatus { get; private set; }

        private ProfileRestored()
        {
        }

        [JsonConstructor]
        protected ProfileRestored(Guid guid, Guid aggregateGuid, int socialProfileStatus)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            SocialProfileStatus = socialProfileStatus;
        }

        public ProfileRestored(Guid aggregateGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            SocialProfileStatus = AggregateStatus.Active.Value;
        }
    }
}