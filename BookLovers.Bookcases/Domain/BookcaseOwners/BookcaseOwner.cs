using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Bookcases.Events.Readers;

namespace BookLovers.Bookcases.Domain.BookcaseOwners
{
    public class BookcaseOwner :
        EventSourcedAggregateRoot,
        IHandle<BookcaseOwnerCreated>,
        IHandle<BookcaseOwnerArchived>
    {
        public int ReaderId { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private BookcaseOwner()
        {
        }

        public BookcaseOwner(Guid aggregateGuid, Guid readerGuid, int readerId, Guid bookcaseGuid)
        {
            Guid = aggregateGuid;
            ReaderGuid = readerGuid;
            ReaderId = readerId;
            BookcaseGuid = bookcaseGuid;
            AggregateStatus = AggregateStatus.Active;

            ApplyChange(BookcaseOwnerCreated.Initialize().WithGuid(aggregateGuid)
                .WithReader(readerGuid, readerId).WithBookcase(bookcaseGuid));
        }

        void IHandle<BookcaseOwnerCreated>.Handle(BookcaseOwnerCreated @event)
        {
            Guid = @event.AggregateGuid;
            ReaderId = @event.ReaderId;
            BookcaseGuid = @event.BookcaseGuid;
            ReaderGuid = @event.ReaderGuid;
            AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<BookcaseOwnerArchived>.Handle(
            BookcaseOwnerArchived @event)
        {
            AggregateStatus = AggregateStatus.Archived;
        }
    }
}