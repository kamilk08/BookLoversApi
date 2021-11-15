using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Queries.Readers
{
    public class ReaderStatisticsQuery : IQuery<ReaderStatisticsDto>
    {
        public int ReaderId { get; set; }

        public ReaderStatisticsQuery()
        {
        }

        public ReaderStatisticsQuery(int readerId)
        {
            this.ReaderId = readerId;
        }
    }
}