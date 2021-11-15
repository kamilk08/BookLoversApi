using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Queries.Readers
{
    public class BlockedReadersQuery : IQuery<IList<ReaderDto>>
    {
        public int ReaderId { get; }

        public BlockedReadersQuery(int readerId)
        {
            this.ReaderId = readerId;
        }
    }
}