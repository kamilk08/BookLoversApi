using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Authors
{
    public class AuthorArchived : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int AuthorStatus { get; private set; }

        public IEnumerable<Guid> AuthorBooks { get; private set; }

        public IEnumerable<Guid> AuthorQuotes { get; private set; }

        private AuthorArchived()
        {
        }

        public AuthorArchived(
            Guid authorGuid,
            IEnumerable<Guid> authorBooks,
            IEnumerable<Guid> authorQuotes)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = authorGuid;
            this.AuthorStatus = AggregateStatus.Archived.Value;
            this.AuthorBooks = authorBooks;
            this.AuthorQuotes = authorQuotes;
        }
    }
}