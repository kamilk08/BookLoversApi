using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Books
{
    public class BookAuthor : ValueObject<BookAuthor>
    {
        public Guid AuthorGuid { get; }

        private BookAuthor()
        {
        }

        public BookAuthor(Guid authorGuid)
        {
            this.AuthorGuid = authorGuid;
        }

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + this.AuthorGuid.GetHashCode();
        }

        protected override bool EqualsCore(BookAuthor obj)
        {
            return this.AuthorGuid == obj.AuthorGuid;
        }
    }
}