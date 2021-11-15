using System;

namespace BookLovers.Publication.Domain.Books.Services
{
    public class BookBasicsData
    {
        public string Title { get; private set; }

        public string Isbn { get; private set; }

        public DateTime PublicationDate { get; private set; }

        public BookCategory BookCategory { get; private set; }

        private BookBasicsData()
        {
        }

        public static BookBasicsData Initialize()
        {
            return new BookBasicsData();
        }

        public BookBasicsData WithTitle(string title)
        {
            this.Title = title;
            return this;
        }

        public BookBasicsData WithIsbn(string isbn)
        {
            this.Isbn = isbn;
            return this;
        }

        public BookBasicsData WithDate(DateTime date)
        {
            this.PublicationDate = date;
            return this;
        }

        public BookBasicsData WithCategory(BookCategory category)
        {
            this.BookCategory = category;
            return this;
        }
    }
}