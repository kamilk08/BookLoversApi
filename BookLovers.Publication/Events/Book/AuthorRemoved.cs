using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class AuthorRemoved : IEvent
    {
        public Guid AuthorGuid { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public bool AddToUnknownAuthor { get; private set; }

        private AuthorRemoved()
        {
        }

        public AuthorRemoved(Guid bookGuid, Guid authorGuid, bool addToUnknownAuthor)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = bookGuid;
            this.AuthorGuid = authorGuid;
            this.AddToUnknownAuthor = addToUnknownAuthor;
        }
    }
}