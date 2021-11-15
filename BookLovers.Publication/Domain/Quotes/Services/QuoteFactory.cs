using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;

namespace BookLovers.Publication.Domain.Quotes.Services
{
    public class QuoteFactory : IDomainService<Quote>
    {
        private readonly IDictionary<QuoteType, IQuoteFactory> _factories;

        public QuoteFactory(IDictionary<QuoteType, IQuoteFactory> factories)
        {
            this._factories = factories;
        }

        public Task<Quote> CreateQuote(
            Guid quoteGuid,
            QuoteContent quoteContent,
            QuoteDetails quoteDetails)
        {
            return this._factories[quoteDetails.QuoteType]
                .Create(quoteGuid, quoteContent, quoteDetails);
        }
    }
}