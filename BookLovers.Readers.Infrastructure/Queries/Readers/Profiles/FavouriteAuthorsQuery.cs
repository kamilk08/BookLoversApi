using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.Profiles
{
    public class FavouriteAuthorsQuery : IQuery<IEnumerable<FavouriteAuthorDto>>
    {
        public int ReaderId { get; set; }

        public FavouriteAuthorsQuery()
        {
        }

        public FavouriteAuthorsQuery(int readerId)
        {
            this.ReaderId = readerId;
        }
    }
}