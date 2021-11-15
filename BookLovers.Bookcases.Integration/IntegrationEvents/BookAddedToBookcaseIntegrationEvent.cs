using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Bookcases.Integration.IntegrationEvents
{
    public class BookAddedToBookcaseIntegrationEvent : IIntegrationEvent
    {
        public Guid AggregateGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid ShelGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        private BookAddedToBookcaseIntegrationEvent()
        {
        }

        private BookAddedToBookcaseIntegrationEvent(
            Guid aggregateGuid,
            Guid bookGuid,
            Guid shelfGuid,
            Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.AggregateGuid = aggregateGuid;
            this.BookGuid = bookGuid;
            this.ShelGuid = shelfGuid;
            this.ReaderGuid = readerGuid;
        }

        public static BookAddedToBookcaseIntegrationEvent Initialize()
        {
            return new BookAddedToBookcaseIntegrationEvent();
        }

        public BookAddedToBookcaseIntegrationEvent WithAggregate(
            Guid aggregateGuid)
        {
            return new BookAddedToBookcaseIntegrationEvent(
                aggregateGuid,
                this.BookGuid, this.ShelGuid, this.ReaderGuid);
        }

        public BookAddedToBookcaseIntegrationEvent WithBook(
            Guid bookGuid)
        {
            return new BookAddedToBookcaseIntegrationEvent(
                this.AggregateGuid, bookGuid, this.ShelGuid,
                this.ReaderGuid);
        }

        public BookAddedToBookcaseIntegrationEvent WithShelf(
            Guid shelfGuid)
        {
            return new BookAddedToBookcaseIntegrationEvent(
                this.AggregateGuid, this.BookGuid, shelfGuid,
                this.ReaderGuid);
        }

        public BookAddedToBookcaseIntegrationEvent WithReader(
            Guid readerGuid)
        {
            return new BookAddedToBookcaseIntegrationEvent(
                this.AggregateGuid, this.BookGuid, this.ShelGuid,
                readerGuid);
        }
    }
}