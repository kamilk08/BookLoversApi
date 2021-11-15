using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Reviews;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.Reviews
{
    public class ReaderBookReviewQuery : IQuery<ReviewDto>
    {
        public int ReaderId { get; set; }

        public int BookId { get; set; }

        public ReaderBookReviewQuery()
        {
        }

        public ReaderBookReviewQuery(int readerId, int bookId)
        {
            this.ReaderId = readerId;
            this.BookId = bookId;
        }
    }
}