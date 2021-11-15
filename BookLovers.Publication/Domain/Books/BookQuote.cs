using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Books
{
    public class BookQuote : ValueObject<BookQuote>
    {
        public Guid QuoteGuid { get; }

        private BookQuote()
        {
        }

        public BookQuote(Guid quoteGuid)
        {
            this.QuoteGuid = quoteGuid;
        }

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + this.QuoteGuid.GetHashCode();
        }

        protected override bool EqualsCore(BookQuote obj)
        {
            return this.QuoteGuid == obj.QuoteGuid;
        }
    }
}