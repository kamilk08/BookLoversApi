using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Auth.Events
{
    public class RegistrationSummaryCompleted : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public Guid UserGuid { get; }

        public DateTime CompletedAt { get; }

        private RegistrationSummaryCompleted()
        {
        }

        [JsonConstructor]
        protected RegistrationSummaryCompleted(
            Guid guid,
            Guid aggregateGuid,
            Guid userGuid,
            DateTime completedAt)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            UserGuid = userGuid;
            CompletedAt = completedAt;
        }

        public RegistrationSummaryCompleted(Guid aggregateGuid, Guid userGuid, DateTime completedAt)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.UserGuid = userGuid;
            this.CompletedAt = completedAt;
        }
    }
}