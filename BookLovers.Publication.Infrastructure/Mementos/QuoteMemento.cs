using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Publication.Domain.Quotes;
using BookLovers.Publication.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Publication.Infrastructure.Mementos
{
    public class QuoteMemento : IQuoteMemento, IMemento<Quote>, IMemento
    {
        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public Guid AuthorGuid { get; private set; }

        [JsonProperty] public Guid BookGuid { get; private set; }

        [JsonProperty] public string QuoteContent { get; private set; }

        [JsonProperty] public Guid AddedByGuid { get; private set; }

        [JsonProperty] public DateTime AddedAt { get; private set; }

        [JsonProperty] public int QuoteTypeId { get; private set; }

        [JsonProperty] public IList<Guid> QuoteLikes { get; private set; }

        public IMemento<Quote> TakeSnapshot(Quote aggregate)
        {
            this.AggregateGuid = aggregate.Guid;
            this.AggregateStatus = aggregate.AggregateStatus.Value;
            this.LastCommittedVersion = aggregate.LastCommittedVersion;
            this.Version = aggregate.Version;

            this.AuthorGuid = aggregate.QuoteDetails.QuoteType == QuoteType.AuthorQuote
                ? aggregate.QuoteContent.QuotedGuid
                : Guid.Empty;

            this.BookGuid = aggregate.QuoteDetails.QuoteType == QuoteType.BookQuote
                ? aggregate.QuoteContent.QuotedGuid
                : Guid.Empty;

            this.QuoteContent = aggregate.QuoteContent.Content;
            this.AddedByGuid = aggregate.QuoteDetails.AddedByGuid;
            this.AddedAt = aggregate.QuoteDetails.AddedAt;
            this.QuoteTypeId = aggregate.QuoteDetails.QuoteType.Value;

            this.QuoteLikes = aggregate.Likes.Select(s => s.ReaderGuid).ToList();
            return this;
        }
    }
}