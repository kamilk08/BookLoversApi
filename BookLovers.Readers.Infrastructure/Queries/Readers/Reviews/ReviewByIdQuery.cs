using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Reviews;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.Reviews
{
    public class ReviewByIdQuery : IQuery<ReviewDto>
    {
        public int ReviewId { get; set; }

        public ReviewByIdQuery()
        {
        }

        public ReviewByIdQuery(int reviewId)
        {
            this.ReviewId = reviewId;
        }
    }
}