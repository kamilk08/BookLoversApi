using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Auth.Events
{
    public class UserArchived : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        private UserArchived()
        {
        }

        [JsonConstructor]
        protected UserArchived(Guid guid, Guid aggregateGuid)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
        }

        public UserArchived(Guid aggregateGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
        }
    }
}