using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;

namespace BookLovers.Bookcases.Infrastructure.Queries.Bookcases
{
    public class BookcaseByReaderGuidQuery : IQuery<BookcaseDto>
    {
        public Guid ReaderGuid { get; }

        public BookcaseByReaderGuidQuery(Guid readerGuid)
        {
            ReaderGuid = readerGuid;
        }
    }
}