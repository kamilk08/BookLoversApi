using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.Queries.Quotes
{
    public class IsQuoteAddedByQuery : IQuery<bool>
    {
        public Guid QuoteGuid { get; }

        public Guid ReaderGuid { get; }

        public IsQuoteAddedByQuery(Guid quoteGuid, Guid readerGuid)
        {
            this.QuoteGuid = quoteGuid;
            this.ReaderGuid = readerGuid;
        }
    }
}