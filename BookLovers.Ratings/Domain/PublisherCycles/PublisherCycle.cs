using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Domain.PublisherCycles.BusinessRules;

namespace BookLovers.Ratings.Domain.PublisherCycles
{
    public class PublisherCycle : AggregateRoot
    {
        public IReadOnlyCollection<Book> Books => this._books.ToList();

        public PublisherCycleIdentification Identification { get; private set; }

        protected ICollection<Book> _books { get; set; } = new List<Book>();

        private PublisherCycle()
        {
        }

        internal PublisherCycle(
            Guid aggregateGuid,
            PublisherCycleIdentification identification)
        {
            this.Guid = aggregateGuid;
            this.Identification = identification;
            this.Status = AggregateStatus.Active.Value;
        }

        public static PublisherCycle Create(PublisherCycleIdentification identification)
        {
            return new PublisherCycle(Guid.NewGuid(), identification);
        }

        public void AddBook(Book book)
        {
            this.CheckBusinessRules(new AddPublisherCycleBookRules(this, book));

            this._books.Add(book);
        }

        public void RemoveBook(Book book)
        {
            this.CheckBusinessRules(new RemovePublisherCycleBookRules(this, book));

            this._books.Remove(book);
        }

        public double Average()
        {
            return this.HasBooks() && this._books.Any(a => a.Ratings.Any())
                ? this._books.Where(p => p.Ratings.Count > 0)
                    .Select(s => s.Average()).Average()
                : 0.0;
        }

        public int RatingsCount()
        {
            return !this.HasBooks()
                ? 0
                : this.Books
                    .Select(s => s.RatingsCount()).Sum();
        }

        private bool HasBooks() => this._books.Any();

        public class Relations
        {
            public const string BooksCollectionName = "_books";

            public static Expression<Func<PublisherCycle, ICollection<Book>>> Books => pc => pc._books;
        }
    }
}