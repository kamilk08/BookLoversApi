using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Books
{
    public class BookBasics : ValueObject<BookBasics>
    {
        public string Title { get; }

        public string ISBN { get; }

        public DateTime PublicationDate { get; }

        public BookCategory BookCategory { get; }

        private BookBasics()
        {
        }

        public BookBasics(
            string isbn,
            string title,
            DateTime publicationDate,
            int category,
            int subCategory)
        {
            this.Title = title;
            this.ISBN = isbn;
            this.PublicationDate = publicationDate;
            this.BookCategory = new BookCategory(category, subCategory);
        }

        public BookBasics(string isbn, string title, DateTime publicationDate, BookCategory category)
        {
            this.Title = title;
            this.ISBN = isbn;
            this.PublicationDate = publicationDate;
            this.BookCategory = category;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.ISBN.GetHashCode();
            hash = (hash * 23) + this.Title.GetHashCode();
            hash = (hash * 23) + this.BookCategory.GetHashCode();
            hash = (hash * 23) + this.PublicationDate.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(BookBasics obj)
        {
            return this.ISBN == obj.ISBN && this.Title == obj.Title &&
                   this.BookCategory == obj.BookCategory &&
                   this.PublicationDate == obj.PublicationDate;
        }
    }
}