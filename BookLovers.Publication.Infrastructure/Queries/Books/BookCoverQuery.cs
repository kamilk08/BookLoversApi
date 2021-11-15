using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.Queries.Books
{
    public class BookCoverQuery : IQuery<Tuple<string, string>>
    {
        public int BookId { get; }

        public BookCoverQuery(int bookId)
        {
            BookId = bookId;
        }
    }
}