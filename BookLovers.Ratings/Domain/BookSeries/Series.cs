using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Domain.BookSeries.BusinessRules;

namespace BookLovers.Ratings.Domain.BookSeries
{
    public class Series : AggregateRoot
    {
        public IReadOnlyCollection<Book> Books => this._books.ToList();

        public SeriesIdentification Identification { get; private set; }

        protected ICollection<Book> _books { get; set; } = (ICollection<Book>) new List<Book>();

        private Series()
        {
        }

        internal Series(Guid aggregateGuid, SeriesIdentification identification)
        {
            this.Guid = aggregateGuid;
            this.Identification = identification;
            this.Status = AggregateStatus.Active.Value;
        }

        public static Series Create(SeriesIdentification identification)
        {
            return new Series(Guid.NewGuid(), identification);
        }

        public void AddBook(Book book)
        {
            this.CheckBusinessRules(new AddBookSeriesRules(this, book));
            this._books.Add(book);
        }

        public void RemoveBook(Book book)
        {
            this.CheckBusinessRules(new RemoveBookSeriesRules(this, book));
            this._books.Remove(book);
        }

        public int RatingsCount()
        {
            return !this.HasBooks()
                ? 0
                : this._books.Select(s => s.Ratings)
                    .Sum(s => s.Count);
        }

        public double Average()
        {
            return this.HasBooks() && this._books.Any(a => a.Ratings.Any())
                ? this._books.Where(p => p.Ratings.Count > 0).Select(s => s.Average()).Average()
                : 0.0;
        }

        private bool HasBooks()
        {
            return this._books.Any();
        }

        public class Relations
        {
            public const string BooksCollectionName = "_books";

            public static Expression<Func<Series, ICollection<Book>>> Books =>
                series => series._books;
        }
    }
}