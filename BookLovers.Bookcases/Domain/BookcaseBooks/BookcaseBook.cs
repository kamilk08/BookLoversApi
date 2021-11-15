using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Bookcases.Events.BookcaseBooks;

namespace BookLovers.Bookcases.Domain.BookcaseBooks
{
    public class BookcaseBook :
        EventSourcedAggregateRoot,
        IHandle<BookcaseBookCreated>,
        IHandle<BookcaseBookArchived>
    {
        public Guid BookGuid { get; private set; }

        public int BookId { get; private set; }

        private BookcaseBook()
        {
        }

        public BookcaseBook(Guid aggregateGuid, Guid bookGuid, int bookId)
        {
            Guid = aggregateGuid;
            BookGuid = bookGuid;
            BookId = bookId;
            AggregateStatus = AggregateStatus.Active;
            ApplyChange(new BookcaseBookCreated(Guid, bookGuid, bookId));
        }

        void IHandle<BookcaseBookCreated>.Handle(BookcaseBookCreated @event)
        {
            Guid = @event.AggregateGuid;
            BookGuid = @event.BookGuid;
            BookId = @event.BookId;
            AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<BookcaseBookArchived>.Handle(BookcaseBookArchived @event)
        {
            AggregateStatus = AggregateStatus.Archived;
        }
    }
}