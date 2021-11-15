using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.Queries.Authors
{
    public class AuthorImageQuery : IQuery<Tuple<string, string>>
    {
        public int AuthorId { get; }

        public AuthorImageQuery(int authorId)
        {
            AuthorId = authorId;
        }
    }
}