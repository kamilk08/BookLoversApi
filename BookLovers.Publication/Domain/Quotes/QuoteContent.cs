using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Quotes
{
    public class QuoteContent : ValueObject<QuoteContent>
    {
        public string Content { get; }

        public Guid QuotedGuid { get; }

        private QuoteContent()
        {
        }

        public QuoteContent(string content, Guid quotedGuid)
        {
            this.Content = content;
            this.QuotedGuid = quotedGuid;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Content.GetHashCode();
            hash = (hash * 23) + this.QuotedGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(QuoteContent obj)
        {
            return this.Content == obj.Content && this.QuotedGuid == obj.QuotedGuid;
        }
    }
}