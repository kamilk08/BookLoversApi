using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class BookPublished : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public DateTime Published { get; private set; }

        private BookPublished()
        {
        }

        public BookPublished(Guid bookId, DateTime published)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = bookId;
            this.Published = published;
        }
    }
}