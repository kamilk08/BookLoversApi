using System;

namespace BookLovers.Publication.Domain.Quotes.Services
{
    public interface IQuoteUniquenessChecker
    {
        bool IsUnique(Guid guid);
    }
}