using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Auth.Events
{
    public class UserPromotedToLibrarianEvent : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        private UserPromotedToLibrarianEvent()
        {
        }

        [JsonConstructor]
        protected UserPromotedToLibrarianEvent(Guid guid, Guid aggregateGuid)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
        }

        public UserPromotedToLibrarianEvent(Guid aggregateGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
        }
    }
}