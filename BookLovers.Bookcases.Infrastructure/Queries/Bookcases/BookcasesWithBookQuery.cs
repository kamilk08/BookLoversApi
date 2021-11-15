using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Queries.Bookcases
{
    public class BookcasesWithBookQuery : IQuery<List<int>>
    {
        public int BookId { get; set; }

        public BookcasesWithBookQuery()
        {
        }

        public BookcasesWithBookQuery(int bookId)
        {
            BookId = bookId;
        }
    }
}