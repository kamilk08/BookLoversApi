using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Ratings.Events
{
    public class BookCreatedEvent : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public IEnumerable<Guid> Authors { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public Guid SeriesGuid { get; private set; }

        public IEnumerable<Guid> Cycles { get; private set; }

        private BookCreatedEvent()
        {
        }

        protected BookCreatedEvent(
            Guid guid,
            Guid aggregateGuid,
            Guid bookGuid,
            IEnumerable<Guid> authors,
            Guid publisherGuid,
            Guid seriesGuid,
            IEnumerable<Guid> cycles)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.BookGuid = bookGuid;
            this.Authors = authors;
            this.PublisherGuid = publisherGuid;
            this.SeriesGuid = seriesGuid;
            this.Cycles = cycles;
        }

        private BookCreatedEvent(
            Guid aggregateGuid,
            Guid bookGuid,
            IEnumerable<Guid> authors,
            Guid publisherGuid,
            Guid seriesGuid,
            IEnumerable<Guid> cycles)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.BookGuid = bookGuid;
            this.Authors = authors;
            this.PublisherGuid = publisherGuid;
            this.SeriesGuid = seriesGuid;
            this.Cycles = cycles;
        }

        public static BookCreatedEvent Initialize()
        {
            return new BookCreatedEvent();
        }

        public BookCreatedEvent WithAggregate(Guid aggregateGuid)
        {
            return new BookCreatedEvent(aggregateGuid, this.BookGuid,
                this.Authors, this.PublisherGuid, this.SeriesGuid, this.Cycles);
        }

        public BookCreatedEvent WithBook(Guid bookGuid)
        {
            return new BookCreatedEvent(this.AggregateGuid, bookGuid,
                this.Authors, this.PublisherGuid, this.SeriesGuid, this.Cycles);
        }

        public BookCreatedEvent WithAuthors(IEnumerable<Guid> authors)
        {
            return new BookCreatedEvent(
                this.AggregateGuid,
                this.BookGuid, authors,
                this.PublisherGuid, this.SeriesGuid, this.Cycles);
        }

        public BookCreatedEvent WithPublisher(Guid publisherGuid)
        {
            return new BookCreatedEvent(
                this.AggregateGuid,
                this.BookGuid, this.Authors, publisherGuid,
                this.SeriesGuid, this.Cycles);
        }

        public BookCreatedEvent WithSeries(Guid seriesGuid)
        {
            return new BookCreatedEvent(
                this.AggregateGuid, this.BookGuid,
                this.Authors, this.PublisherGuid,
                seriesGuid, this.Cycles);
        }

        public BookCreatedEvent WithCycles(IEnumerable<Guid> cycles)
        {
            return new BookCreatedEvent(
                this.AggregateGuid,
                this.BookGuid, this.Authors,
                this.PublisherGuid, this.SeriesGuid, cycles);
        }
    }
}