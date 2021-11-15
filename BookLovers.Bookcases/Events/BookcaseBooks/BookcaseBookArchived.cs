using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.BookcaseBooks
{
    public class BookcaseBookArchived : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int BookId { get; private set; }

        public int Status { get; private set; }

        private BookcaseBookArchived()
        {
        }

        public BookcaseBookArchived(Guid aggregateGuid, int bookId)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            BookId = bookId;
            Status = AggregateStatus.Archived.Value;
        }
    }
}