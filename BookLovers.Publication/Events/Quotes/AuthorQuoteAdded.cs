using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Publication.Domain.Quotes;

namespace BookLovers.Publication.Events.Quotes
{
    public class AuthorQuoteAdded : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public Guid AddedBy { get; private set; }

        public string Quote { get; private set; }

        public int Type { get; private set; }

        public int Likes { get; private set; }

        public DateTime AddedAt { get; private set; }

        public int QuoteState { get; private set; }

        private AuthorQuoteAdded()
        {
        }

        private AuthorQuoteAdded(
            Guid aggregateGuid,
            Guid authorGuid,
            Guid addedBy,
            string quote,
            DateTime addedAt)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.AuthorGuid = authorGuid;
            this.AddedBy = addedBy;
            this.Quote = quote;
            this.Likes = 0;
            this.Type = QuoteType.AuthorQuote.Value;
            this.QuoteState = AggregateStatus.Active.Value;
            this.AddedAt = addedAt;
        }

        public static AuthorQuoteAdded Initialize()
        {
            return new AuthorQuoteAdded();
        }

        public AuthorQuoteAdded WithAggregate(Guid aggregateGuid)
        {
            return new AuthorQuoteAdded(
                aggregateGuid, this.AuthorGuid, this.AddedBy, this.Quote, this.AddedAt);
        }

        public AuthorQuoteAdded WithAuthor(Guid authorGuid)
        {
            return new AuthorQuoteAdded(this.AggregateGuid, authorGuid,
                this.AddedBy, this.Quote, this.AddedAt);
        }

        public AuthorQuoteAdded WithQuote(string quote, DateTime addedAt)
        {
            return new AuthorQuoteAdded(this.AggregateGuid, this.AuthorGuid, this.AddedBy, quote, addedAt);
        }

        public AuthorQuoteAdded WithAddedBy(Guid addedByGuid)
        {
            return new AuthorQuoteAdded(
                this.AggregateGuid,
                this.AuthorGuid, addedByGuid, this.Quote, this.AddedAt);
        }
    }
}