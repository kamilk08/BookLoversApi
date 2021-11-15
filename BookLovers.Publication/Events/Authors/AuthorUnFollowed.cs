using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Authors
{
    public class AuthorUnFollowed : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid FollowedBy { get; private set; }

        private AuthorUnFollowed()
        {
        }

        public AuthorUnFollowed(Guid aggregateGuid, Guid followedBy)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.FollowedBy = followedBy;
        }
    }
}