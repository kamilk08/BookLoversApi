using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Queries.Readers
{
    public class ReaderByIdQuery : IQuery<ReaderDto>
    {
        public int ReaderId { get; set; }

        public ReaderByIdQuery()
        {
        }

        public ReaderByIdQuery(int readerId)
        {
            this.ReaderId = readerId;
        }
    }
}