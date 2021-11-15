using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.TimeLines
{
    public class ReaderTimeLineQuery : IQuery<TimeLineDto>
    {
        public int ReaderId { get; set; }

        public ReaderTimeLineQuery()
        {
        }

        public ReaderTimeLineQuery(int readerId)
        {
            this.ReaderId = readerId;
        }
    }
}