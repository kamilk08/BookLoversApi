using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class BookDetailsChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int? Pages { get; private set; }

        public int? Language { get; private set; }

        private BookDetailsChanged()
        {
        }

        public BookDetailsChanged(Guid bookGuid, int? pages, int? language)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = bookGuid;
            this.Pages = pages;
            this.Language = language;
        }
    }
}