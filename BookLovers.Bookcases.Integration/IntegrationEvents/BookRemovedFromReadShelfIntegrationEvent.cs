using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Bookcases.Integration.IntegrationEvents
{
    public class BookRemovedFromReadShelfIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private BookRemovedFromReadShelfIntegrationEvent()
        {
        }

        public BookRemovedFromReadShelfIntegrationEvent(
            Guid aggregateGuid,
            Guid readerGuid,
            Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.AggregateGuid = aggregateGuid;
            this.ReaderGuid = readerGuid;
            this.BookGuid = bookGuid;
        }
    }
}