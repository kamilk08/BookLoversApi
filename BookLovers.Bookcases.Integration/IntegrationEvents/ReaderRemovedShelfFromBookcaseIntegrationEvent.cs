using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Bookcases.Integration.IntegrationEvents
{
    public class ReaderRemovedShelfFromBookcaseIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public IList<Guid> BooksOnShelf { get; private set; }

        public Guid ShelfGuid { get; private set; }

        private ReaderRemovedShelfFromBookcaseIntegrationEvent()
        {
        }

        public ReaderRemovedShelfFromBookcaseIntegrationEvent(IList<Guid> booksOnShelf, Guid shelfGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.BooksOnShelf = booksOnShelf;
            this.ShelfGuid = shelfGuid;
        }
    }
}