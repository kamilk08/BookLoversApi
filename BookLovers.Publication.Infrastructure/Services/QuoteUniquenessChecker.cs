using System;
using System.Linq;
using BookLovers.Publication.Domain.Quotes.Services;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Services
{
    internal class QuoteUniquenessChecker : IQuoteUniquenessChecker
    {
        private readonly PublicationsContext _context;

        public QuoteUniquenessChecker(PublicationsContext context)
        {
            this._context = context;
        }

        public bool IsUnique(Guid guid)
        {
            return !this._context.Quotes.AsNoTracking()
                .Any(a => a.Guid == guid);
        }
    }
}