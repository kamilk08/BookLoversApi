using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.BookcaseBooks
{
    public class BookcaseBookCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public int BookId { get; private set; }

        public int Status { get; private set; }

        private BookcaseBookCreated()
        {
        }

        public BookcaseBookCreated(Guid aggregateGuid, Guid bookGuid, int bookId)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            BookGuid = bookGuid;
            BookId = bookId;
            Status = AggregateStatus.Active.Value;
        }
    }
}