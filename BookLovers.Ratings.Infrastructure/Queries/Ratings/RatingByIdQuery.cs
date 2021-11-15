using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Ratings
{
    public class RatingByIdQuery : IQuery<RatingDto>
    {
        public int RatingId { get; set; }

        public RatingByIdQuery()
        {
        }

        public RatingByIdQuery(int ratingId)
        {
            this.RatingId = ratingId;
        }
    }
}