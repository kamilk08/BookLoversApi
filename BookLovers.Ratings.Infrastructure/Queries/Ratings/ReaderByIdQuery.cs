using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Ratings
{
    public class ReaderByIdQuery : IQuery<ReaderDto>
    {
        public int ReaderId { get; }

        public ReaderByIdQuery(int readerId)
        {
            this.ReaderId = readerId;
        }
    }
}