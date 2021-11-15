using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class AuthorAdded : IEvent
    {
        public Guid AuthorGuid { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        private AuthorAdded()
        {
        }

        public AuthorAdded(Guid bookGuid, Guid authorGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = bookGuid;
            this.AuthorGuid = authorGuid;
        }
    }
}