using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Ratings.Domain.Books
{
    public class BookIdentification : ValueObject<BookIdentification>
    {
        public Guid BookGuid { get; private set; }

        public int BookId { get; private set; }

        private BookIdentification()
        {
        }

        public BookIdentification(Guid bookGuid, int bookId)
        {
            this.BookGuid = bookGuid;
            this.BookId = bookId;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.BookId.GetHashCode();
            hash = (hash * 23) + this.BookGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(BookIdentification obj)
        {
            return this.BookGuid == obj.BookGuid && this.BookId == obj.BookId;
        }
    }
}