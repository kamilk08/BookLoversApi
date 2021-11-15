using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Auth.Events
{
    public class UserCreated : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public string Email { get; }

        private UserCreated()
        {
        }

        [JsonConstructor]
        protected UserCreated(Guid guid, Guid aggregateGuid, string email)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.Email = email;
        }

        public UserCreated(Guid aggregateGuid, string email)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.Email = email;
        }
    }
}