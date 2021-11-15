using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Books
{
    public class BookCreatedIntegrationEvent : IIntegrationEvent
    {
        public Guid BookGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int BookId { get; private set; }

        public IEnumerable<Guid> Authors { get; private set; }

        public Guid SeriesGuid { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public IEnumerable<Guid> Cycles { get; private set; }

        public int BookStatus { get; private set; }

        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        private BookCreatedIntegrationEvent()
        {
        }

        private BookCreatedIntegrationEvent(
            Guid bookGuid,
            int bookId,
            Guid readerGuid,
            Guid seriesGuid,
            Guid publisherGuid,
            IEnumerable<Guid> authors,
            IEnumerable<Guid> cycles)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.BookGuid = bookGuid;
            this.BookId = bookId;
            this.Authors = authors;
            this.Cycles = cycles;
            this.PublisherGuid = publisherGuid;
            this.ReaderGuid = readerGuid;
            this.SeriesGuid = seriesGuid;
            this.BookStatus = AggregateStatus.Active.Value;
        }

        public static BookCreatedIntegrationEvent Initialize()
        {
            return new BookCreatedIntegrationEvent();
        }

        public BookCreatedIntegrationEvent WithBook(
            Guid bookGuid,
            int bookId)
        {
            return new BookCreatedIntegrationEvent(bookGuid, bookId, this.ReaderGuid, this.SeriesGuid,
                this.PublisherGuid, this.Authors, this.Cycles);
        }

        public BookCreatedIntegrationEvent WithSeries(Guid seriesGuid)
        {
            return new BookCreatedIntegrationEvent(
                this.BookGuid,
                this.BookId, this.ReaderGuid, seriesGuid, this.PublisherGuid, this.Authors, this.Cycles);
        }

        public BookCreatedIntegrationEvent WithPublisher(Guid publisherGuid)
        {
            return new BookCreatedIntegrationEvent(
                this.BookGuid, this.BookId, this.ReaderGuid, this.SeriesGuid, publisherGuid, this.Authors, this.Cycles);
        }

        public BookCreatedIntegrationEvent WithAuthors(
            IEnumerable<Guid> authors)
        {
            return new BookCreatedIntegrationEvent(this.BookGuid, this.BookId, this.ReaderGuid, this.SeriesGuid,
                this.PublisherGuid, authors, this.Cycles);
        }

        public BookCreatedIntegrationEvent WithCycles(
            IEnumerable<Guid> cycles)
        {
            return new BookCreatedIntegrationEvent(this.BookGuid, this.BookId, this.ReaderGuid, this.SeriesGuid,
                this.PublisherGuid, this.Authors, cycles);
        }

        public BookCreatedIntegrationEvent WithReader(Guid readerGuid)
        {
            return new BookCreatedIntegrationEvent(
                this.BookGuid,
                this.BookId, readerGuid, this.SeriesGuid, this.PublisherGuid, this.Authors, this.Cycles);
        }
    }
}