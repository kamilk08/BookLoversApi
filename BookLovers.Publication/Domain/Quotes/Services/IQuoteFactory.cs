using System;
using System.Threading.Tasks;

namespace BookLovers.Publication.Domain.Quotes.Services
{
    public interface IQuoteFactory
    {
        QuoteType QuoteType { get; }

        Task<Quote> Create(Guid quoteGuid, QuoteContent quoteContent, QuoteDetails quoteDetails);
    }
}