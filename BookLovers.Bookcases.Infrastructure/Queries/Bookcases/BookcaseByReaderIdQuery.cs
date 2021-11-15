using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;

namespace BookLovers.Bookcases.Infrastructure.Queries.Bookcases
{
    public class BookcaseByReaderIdQuery : IQuery<BookcaseDto>
    {
        public int ReaderId { get; set; }

        public BookcaseByReaderIdQuery()
        {
        }

        public BookcaseByReaderIdQuery(int readerId)
        {
            ReaderId = readerId;
        }
    }
}