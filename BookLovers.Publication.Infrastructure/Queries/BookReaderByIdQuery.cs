using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos;

namespace BookLovers.Publication.Infrastructure.Queries
{
    public class BookReaderByIdQuery : IQuery<BookReaderDto>
    {
        public int ReaderId { get; }

        public BookReaderByIdQuery(int readerId)
        {
            this.ReaderId = readerId;
        }
    }
}