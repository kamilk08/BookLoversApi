using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Ratings
{
    public class ReaderBookRatingQuery : IQuery<RatingDto>
    {
        public int ReaderId { get; set; }

        public int BookId { get; set; }

        public ReaderBookRatingQuery()
        {
        }

        public ReaderBookRatingQuery(int bookId, int readerId)
        {
            this.BookId = bookId;
            this.ReaderId = readerId;
        }
    }
}