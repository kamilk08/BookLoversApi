using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Readers
{
    public class BookcaseOwnerCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int ReaderId { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int Status { get; private set; }

        private BookcaseOwnerCreated()
        {
        }

        private BookcaseOwnerCreated(
            Guid aggregateGuid,
            int readerId,
            Guid bookcaseGuid,
            Guid readerGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReaderId = readerId;
            BookcaseGuid = bookcaseGuid;
            ReaderGuid = readerGuid;
            Status = AggregateStatus.Active.Value;
        }

        public static BookcaseOwnerCreated Initialize()
        {
            return new BookcaseOwnerCreated();
        }

        public BookcaseOwnerCreated WithGuid(Guid guid)
        {
            return new BookcaseOwnerCreated(guid, ReaderId, BookcaseGuid, ReaderGuid);
        }

        public BookcaseOwnerCreated WithReader(Guid readerGuid, int readerId)
        {
            return new BookcaseOwnerCreated(Guid, readerId, BookcaseGuid, readerGuid);
        }

        public BookcaseOwnerCreated WithBookcase(Guid bookcaseGuid)
        {
            return new BookcaseOwnerCreated(Guid, ReaderId, bookcaseGuid, ReaderGuid);
        }
    }
}