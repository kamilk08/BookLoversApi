using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Authors
{
    public class AuthorRatingsQuery : IQuery<RatingsDto>
    {
        public int AuthorId { get; set; }

        public AuthorRatingsQuery()
        {
        }

        public AuthorRatingsQuery(int authorId)
        {
            this.AuthorId = authorId;
        }
    }
}