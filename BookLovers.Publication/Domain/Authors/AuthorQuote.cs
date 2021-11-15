using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Authors
{
    public class AuthorQuote : ValueObject<AuthorQuote>
    {
        public Guid QuoteGuid { get; }

        private AuthorQuote()
        {
        }

        public AuthorQuote(Guid quoteGuid)
        {
            this.QuoteGuid = quoteGuid;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.QuoteGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(AuthorQuote obj)
        {
            return this.QuoteGuid == obj.QuoteGuid;
        }
    }
}