using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Authors
{
    public class AuthorBookRemoved : IEvent
    {
        public Guid BookGuid { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        private AuthorBookRemoved()
        {
        }

        public AuthorBookRemoved(Guid authorGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = authorGuid;
            this.BookGuid = bookGuid;
        }
    }
}