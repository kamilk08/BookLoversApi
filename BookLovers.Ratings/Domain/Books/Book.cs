using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Ratings.Domain.Authors;
using BookLovers.Ratings.Domain.Books.BusinessRules;
using BookLovers.Ratings.Domain.RatingGivers;
using BookLovers.Ratings.Domain.RatingStars;
using BookLovers.Ratings.Events;

namespace BookLovers.Ratings.Domain.Books
{
    public class Book : AggregateRoot
    {
        public BookIdentification Identification { get; private set; }

        public IReadOnlyList<Rating> Ratings => this._ratings.ToList();

        public IReadOnlyList<Author> Authors => this._authors.ToList();

        protected ICollection<Rating> _ratings { get; set; } = new List<Rating>();

        protected ICollection<Author> _authors { get; set; } = new List<Author>();

        private Book()
        {
        }

        internal Book(
            Guid aggregateGuid,
            BookIdentification identification,
            IEnumerable<Author> authors)
        {
            this.Guid = aggregateGuid;
            this.Identification = identification;
            this._authors = authors.ToList();
            this.Status = AggregateStatus.Active.Value;
        }

        public static Book Create(BookIdentification identification, IEnumerable<Author> authors) =>
            new Book(Guid.NewGuid(), identification, authors);

        public void AddRating(Rating rating)
        {
            this.CheckBusinessRules(new AddRatingRules(this, rating));

            this._ratings.Add(rating);

            this.AddEvent(new BookRatingChanged(this.Identification.BookGuid));
        }

        public void ChangeRating(RatingGiver ratingGiver, Rating newRating)
        {
            this.CheckBusinessRules(new EditRatingRules(this, ratingGiver, newRating));

            var readerRating = this.GetReaderRating(ratingGiver.ReaderId);

            this._ratings.Remove(readerRating);

            readerRating.ChangeRating(newRating.Stars);

            this._ratings.Add(readerRating);

            this.AddEvent(new BookRatingChanged(this.Identification.BookGuid));
        }

        public void RemoveRating(Rating rating)
        {
            this.CheckBusinessRules(new RemoveRatingRules(this, rating));

            this._ratings.Remove(rating);

            this.AddEvent(new BookRatingChanged(this.Identification.BookGuid));
        }

        public Rating GetReaderRating(int readerId)
        {
            return this._ratings.SingleOrDefault(p => p.ReaderId == readerId);
        }

        public bool HasRating(int readerId)
        {
            return this.GetReaderRating(readerId) != null;
        }

        public int RatingsCount()
        {
            return this._ratings.Count != 0 ? this._ratings.Count(p => p.Stars > Star.Zero.Value) : 0;
        }

        public double Average()
        {
            return this._ratings.Count != 0
                ? this._ratings.Select(s => s.Stars).Where(p => p > Star.Zero.Value).Average(p => p)
                : 0.0;
        }

        public class Relations
        {
            public const string RatingsCollectionName = "_ratings";
            public const string AuthorsCollectionName = "_authors";

            public static Expression<Func<Book, ICollection<Rating>>> Ratings => b => b._ratings;

            public static Expression<Func<Book, ICollection<Author>>> Authors => b => b._authors;
        }
    }
}