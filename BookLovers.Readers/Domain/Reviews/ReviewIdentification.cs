using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Reviews
{
    public class ReviewIdentification : ValueObject<ReviewIdentification>
    {
        public Guid BookGuid { get; }

        public Guid ReaderGuid { get; }

        private ReviewIdentification()
        {
        }

        public ReviewIdentification(Guid bookGuid, Guid readerGuid)
        {
            BookGuid = bookGuid;
            ReaderGuid = readerGuid;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.BookGuid.GetHashCode();
            hash = (hash * 23) + this.ReaderGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(ReviewIdentification obj) =>
            BookGuid == obj.BookGuid && ReaderGuid == obj.ReaderGuid;
    }
}