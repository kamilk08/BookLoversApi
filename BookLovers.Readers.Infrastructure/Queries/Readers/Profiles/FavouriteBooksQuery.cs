using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.Profiles
{
    public class FavouriteBooksQuery : IQuery<IEnumerable<FavouriteBookDto>>
    {
        public int ReaderId { get; set; }

        public FavouriteBooksQuery()
        {
        }

        public FavouriteBooksQuery(int readerId)
        {
            this.ReaderId = readerId;
        }
    }
}