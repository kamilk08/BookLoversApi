using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookLovers.Base.Domain.Aggregates;

namespace BookLovers.Ratings.Domain.RatingGivers
{
    public class RatingGiver : AggregateRoot
    {
        private List<Rating> _ratings = new List<Rating>();

        public int ReaderId { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public IReadOnlyList<Rating> Ratings => this._ratings.ToList();

        private RatingGiver()
        {
        }

        public RatingGiver(Guid aggregateGuid, Guid readerGuid, int readerId)
        {
            this.Guid = aggregateGuid;
            this.ReaderGuid = readerGuid;
            this.ReaderId = readerId;
            this.Status = AggregateStatus.Active.Value;
        }

        public void AddRating(Rating rating)
        {
            this._ratings.Add(rating);
        }

        public class RatingGiverRelations
        {
            private const string RatingsName = "_ratings";
            public static Expression<Func<RatingGiver, ICollection<Rating>>> Ratings = rg => rg._ratings;
        }
    }
}