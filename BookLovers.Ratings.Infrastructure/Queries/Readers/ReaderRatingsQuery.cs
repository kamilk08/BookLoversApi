using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Readers
{
    public class ReaderRatingsQuery : IQuery<ReaderRatingsDto>
    {
        public int ReaderId { get; set; }

        public ReaderRatingsQuery()
        {
        }

        public ReaderRatingsQuery(int readerId)
        {
            this.ReaderId = readerId;
        }
    }
}