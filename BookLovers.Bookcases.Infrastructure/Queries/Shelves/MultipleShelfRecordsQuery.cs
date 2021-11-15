using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;

namespace BookLovers.Bookcases.Infrastructure.Queries.Shelves
{
    public class MultipleShelfRecordsQuery : IQuery<List<ShelfRecordDto>>
    {
        public int BookcaseId { get; set; }

        public List<int> BookIds { get; set; }

        public MultipleShelfRecordsQuery()
        {
            BookIds = BookIds ?? new List<int>();
        }

        public MultipleShelfRecordsQuery(List<int> bookIds, int bookcaseId)
        {
            BookIds = bookIds;
            BookcaseId = bookcaseId;
        }
    }
}