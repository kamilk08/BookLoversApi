using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Ratings.Domain.Authors.BusinessRules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.Authors
{
    public class Author : AggregateRoot
    {
        protected ICollection<Book> _books { get; set; } = new List<Book>();

        public AuthorIdentification Identification { get; private set; }

        public IReadOnlyCollection<Book> Books => this._books.ToList();

        private Author()
        {
        }

        internal Author(Guid aggregateGuid, AuthorIdentification identification)
        {
            this.Guid = aggregateGuid;
            this.Identification = identification;
            this.Status = AggregateStatus.Active.Value;
        }

        public static Author Create(AuthorIdentification identification)
        {
            return new Author(Guid.NewGuid(), identification);
        }

        public void AddBook(Book book)
        {
            this.CheckBusinessRules(new AddAuthorBookRules(this, book));

            this._books.Add(book);
        }

        public void RemoveBook(Book book)
        {
            this.CheckBusinessRules(new RemoveAuthorBookRules(this, book));
            this._books.Remove(book);
        }

        public double Average()
        {
            return this.HasBooks && this._books.Any(a => a.Ratings.Any())
                ? this._books.Where(p => p.Ratings.Count > 0)
                    .Select(s => s.Average()).Average()
                : 0.0;
        }

        public int RatingsCount()
        {
            return !this.HasBooks
                ? 0
                : this._books.Select(s => s.RatingsCount()).Sum();
        }

        private bool HasBooks => this._books.Any();

        public class Relations
        {
            public const string BooksCollectionName = "_books";

            public static Expression<Func<Author, ICollection<Book>>> Books =
                author => author._books;
        }
    }
}