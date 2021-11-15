using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Publication.Domain.Quotes;

namespace BookLovers.Publication.Events.Quotes
{
    public class BookQuoteAdded : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid AddedBy { get; private set; }

        public int Type { get; private set; }

        public string Quote { get; private set; }

        public int Likes { get; private set; }

        public DateTime AddedAt { get; private set; }

        public int QuoteState { get; private set; }

        private BookQuoteAdded()
        {
        }

        private BookQuoteAdded(
            Guid aggregateGuid,
            Guid bookGuid,
            Guid addedBy,
            string quote,
            DateTime addedAt)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.BookGuid = bookGuid;
            this.AddedBy = addedBy;
            this.Quote = quote;
            this.Likes = 0;
            this.Type = QuoteType.BookQuote.Value;
            this.QuoteState = AggregateStatus.Active.Value;
            this.AddedAt = addedAt;
        }

        public static BookQuoteAdded Initialize()
        {
            return new BookQuoteAdded();
        }

        public BookQuoteAdded WithAggregate(Guid aggregateGuid)
        {
            return new BookQuoteAdded(aggregateGuid, this.BookGuid,
                this.AddedBy, this.Quote, this.AddedAt);
        }

        public BookQuoteAdded WithBook(Guid bookGuid)
        {
            return new BookQuoteAdded(this.AggregateGuid, bookGuid, this.AddedBy, this.Quote, this.AddedAt);
        }

        public BookQuoteAdded WithQuote(string quote, DateTime addedAt)
        {
            return new BookQuoteAdded(this.AggregateGuid, this.BookGuid, this.AddedBy, quote, addedAt);
        }

        public BookQuoteAdded WithAddedBy(Guid addedByGuid)
        {
            return new BookQuoteAdded(this.AggregateGuid, this.BookGuid,
                addedByGuid, this.Quote, this.AddedAt);
        }
    }
}