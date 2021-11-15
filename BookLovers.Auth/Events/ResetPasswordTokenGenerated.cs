using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Auth.Events
{
    public class ResetPasswordTokenGenerated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public string Token { get; private set; }

        [JsonConstructor]
        protected ResetPasswordTokenGenerated(Guid guid, Guid aggregateGuid, string token)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            Token = token;
        }

        public ResetPasswordTokenGenerated(Guid aggregateGuid, string token)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            Token = token;
        }
    }
}