using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Authors
{
    public class AuthorBookAdded : IEvent
    {
        public Guid BookGuid { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        private AuthorBookAdded()
        {
        }

        public AuthorBookAdded(Guid authorGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = authorGuid;
            this.BookGuid = bookGuid;
        }
    }
}