using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Quotes
{
    public class QuoteDetails : ValueObject<QuoteDetails>
    {
        public Guid AddedByGuid { get; }

        public DateTime AddedAt { get; }

        public QuoteType QuoteType { get; }

        private QuoteDetails()
        {
        }

        public QuoteDetails(Guid addedByGuid, DateTime addedAt, QuoteType quoteType)
        {
            this.AddedByGuid = addedByGuid;
            this.AddedAt = addedAt;
            this.QuoteType = quoteType;
        }

        protected override bool EqualsCore(QuoteDetails obj)
        {
            return this.AddedByGuid == obj.AddedByGuid
                   && this.AddedAt == obj.AddedAt &&
                   this.QuoteType.Value == obj.QuoteType.Value;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.AddedByGuid.GetHashCode();
            hash = (hash * 23) + this.AddedAt.GetHashCode();
            hash = (hash * 23) + this.QuoteType.GetHashCode();

            return hash;
        }
    }
}