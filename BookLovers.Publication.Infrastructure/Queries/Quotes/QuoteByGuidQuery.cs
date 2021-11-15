using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Quotes
{
    public class QuoteByGuidQuery : IQuery<QuoteDto>
    {
        public Guid QuoteGuid { get; }

        public QuoteByGuidQuery(Guid quoteGuid)
        {
            this.QuoteGuid = quoteGuid;
        }
    }
}