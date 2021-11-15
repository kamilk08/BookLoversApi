using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Queries.Readers
{
    public class ReaderByGuidQuery : IQuery<ReaderDto>
    {
        public Guid Guid { get; set; }

        public ReaderByGuidQuery()
        {
        }

        public ReaderByGuidQuery(Guid guid)
        {
            this.Guid = guid;
        }
    }
}