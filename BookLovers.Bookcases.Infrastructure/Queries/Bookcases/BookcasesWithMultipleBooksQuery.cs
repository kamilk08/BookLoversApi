using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Queries.Bookcases
{
    public class BookcasesWithMultipleBooksQuery : IQuery<List<int>>
    {
        public List<int> BookIds { get; set; }

        public BookcasesWithMultipleBooksQuery()
        {
            BookIds = BookIds ?? new List<int>();
        }

        public BookcasesWithMultipleBooksQuery(List<int> bookIds)
        {
            BookIds = bookIds;
        }
    }
}