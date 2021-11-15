using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Books
{
    public class MultipleBooksRatingsQuery : IQuery<List<BookDto>>
    {
        public List<int> BookIds { get; set; }

        public MultipleBooksRatingsQuery()
        {
            this.BookIds = this.BookIds ?? new List<int>();
        }

        public MultipleBooksRatingsQuery(List<int> bookIds)
        {
            this.BookIds = bookIds;
        }
    }
}