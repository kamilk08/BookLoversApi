using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Domain.Publisher.BusinessRules;
using BookLovers.Ratings.Domain.PublisherCycles;

namespace BookLovers.Ratings.Domain.Publisher
{
    public class Publisher : AggregateRoot
    {
        public PublisherIdentification Identification { get; private set; }

        public IReadOnlyCollection<Book> Books => _books.ToList();

        public IReadOnlyCollection<PublisherCycle> PublisherCycles => _cycles.ToList();

        protected ICollection<Book> _books { get; set; } = new List<Book>();

        protected ICollection<PublisherCycle> _cycles { get; set; } = new List<PublisherCycle>();

        private Publisher()
        {
        }

        internal Publisher(Guid aggregateGuid, PublisherIdentification identification)
        {
            Guid = aggregateGuid;
            Identification = identification;
            Status = AggregateStatus.Active.Value;
        }

        public static Publisher Create(
            PublisherIdentification identification)
        {
            return new Publisher(Guid.NewGuid(), identification);
        }

        public void AddBook(Book book)
        {
            CheckBusinessRules(new AddPublisherBookRules(this, book));

            _books.Add(book);
        }

        public void RemoveBook(Book book)
        {
            CheckBusinessRules(new RemovePublisherBookRules(this, book));

            _books.Remove(book);
        }

        public void AddCycle(PublisherCycle cycle)
        {
            CheckBusinessRules(new AddCycleRules(this, cycle));

            _cycles.Add(cycle);
        }

        public void RemoveCycle(PublisherCycle cycle)
        {
            CheckBusinessRules(new RemoveCycleRules(this, cycle));

            _cycles.Remove(cycle);
        }

        public PublisherCycle GetPublisherCycle(Guid guid) =>
            _cycles.SingleOrDefault(p => p.Identification.CycleGuid == guid);

        public double Average()
        {
            return HasBooks() && _books.Any(a => a.Ratings.Any())
                ? _books.Where(p => p.Ratings.Count > 0)
                    .Select(s => s.Average()).Average()
                : 0.0;
        }

        public int RatingsCount() => _books.Select(s => s.RatingsCount()).Sum();

        private bool HasBooks() => _books.Any();

        public class Relations
        {
            public const string BooksCollectionName = "_books";
            public const string CyclesCollectionName = "_cycles";

            public static Expression<Func<Publisher, ICollection<Book>>> Books =
                p => p._books;

            public static Expression<Func<Publisher, ICollection<PublisherCycle>>>
                Cycles = pc => pc._cycles;
        }
    }
}