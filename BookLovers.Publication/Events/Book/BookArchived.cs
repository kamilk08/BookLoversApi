using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class BookArchived : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int BookStatus { get; private set; }

        public IEnumerable<Guid> Authors { get; private set; }

        public IEnumerable<Guid> Quotes { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public Guid? SeriesGuid { get; private set; }

        public int? PositionInSeries { get; private set; }

        private BookArchived()
        {
        }

        private BookArchived(
            Guid bookGuid,
            Guid publisherGuid,
            Guid? seriesGuid,
            int? positionInSeries,
            IEnumerable<Guid> authors,
            IEnumerable<Guid> quotes)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = bookGuid;
            this.BookStatus = AggregateStatus.Archived.Value;
            this.Authors = authors;
            this.PublisherGuid = publisherGuid;
            this.SeriesGuid = seriesGuid;
            this.PositionInSeries = positionInSeries;
            this.Quotes = quotes;
        }

        public static BookArchived Initialize()
        {
            return new BookArchived();
        }

        public BookArchived WithAggregate(Guid bookGuid)
        {
            return new BookArchived(bookGuid, this.PublisherGuid,
                this.SeriesGuid, this.PositionInSeries, this.Authors, this.Quotes);
        }

        public BookArchived WithPublisher(Guid publisherGuid)
        {
            return new BookArchived(this.AggregateGuid, publisherGuid,
                this.SeriesGuid, this.PositionInSeries, this.Authors, this.Quotes);
        }

        public BookArchived WithSeries(Guid? seriesGuid, int? positionInSeries)
        {
            return new BookArchived(
                this.AggregateGuid,
                this.PublisherGuid, seriesGuid, positionInSeries, this.Authors, this.Quotes);
        }

        public BookArchived WithAuthors(IEnumerable<Guid> authors)
        {
            return new BookArchived(
                this.AggregateGuid,
                this.PublisherGuid, this.SeriesGuid, this.PositionInSeries, authors, this.Quotes);
        }

        public BookArchived WithQuotes(IEnumerable<Guid> quotes)
        {
            return new BookArchived(
                this.AggregateGuid,
                this.PublisherGuid, this.SeriesGuid, this.PositionInSeries, this.Authors, quotes);
        }
    }
}