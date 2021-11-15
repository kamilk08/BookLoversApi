using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Books
{
    public class BookReview : ValueObject<BookReview>
    {
        public Guid ReaderGuid { get; }

        public Guid ReviewGuid { get; }

        private BookReview()
        {
        }

        public BookReview(Guid readerGuid, Guid reviewGuid)
        {
            this.ReaderGuid = readerGuid;
            this.ReviewGuid = reviewGuid;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.ReaderGuid.GetHashCode();
            hash = (hash * 23) + this.ReviewGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(BookReview obj)
        {
            return this.ReaderGuid == obj.ReaderGuid && this.ReviewGuid == obj.ReviewGuid;
        }
    }
}