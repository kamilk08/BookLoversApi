using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Readers
{
    public class ReaderReview : ValueObject<ReaderReview>, IAddedResource
    {
        public Guid ReviewGuid { get; }

        public Guid BookGuid { get; }

        public DateTime AddedAt { get; }

        public Guid ResourceGuid => ReviewGuid;

        public AddedResourceType AddedResourceType => AddedResourceType.Review;

        private ReaderReview()
        {
        }

        public ReaderReview(Guid reviewGuid, Guid bookGuid, DateTime addedAt)
        {
            ReviewGuid = reviewGuid;
            BookGuid = bookGuid;
            AddedAt = addedAt;
        }

        protected override bool EqualsCore(ReaderReview obj)
        {
            return ReviewGuid == obj.ReviewGuid && BookGuid == obj.BookGuid
                                                && AddedAt == obj.AddedAt;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.ReviewGuid.GetHashCode();
            hash = (hash * 23) + this.BookGuid.GetHashCode();
            hash = (hash * 23) + this.AddedAt.GetHashCode();

            return hash;
        }
    }
}