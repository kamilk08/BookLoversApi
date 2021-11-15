using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Auth.Events
{
    public class RegistrationSummaryNotCompleted : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public Guid UserGuid { get; }

        private RegistrationSummaryNotCompleted()
        {
        }

        [JsonConstructor]
        protected RegistrationSummaryNotCompleted(Guid guid, Guid aggregateGuid, Guid userGuid)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.UserGuid = userGuid;
        }

        public RegistrationSummaryNotCompleted(Guid aggregateGuid, Guid userGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.UserGuid = userGuid;
        }
    }
}