using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Authors
{
    public class AuthorFollowersQuery : IQuery<List<AuthorFollowerDto>>
    {
        public int AuthorId { get; }

        public AuthorFollowersQuery(int authorId)
        {
            this.AuthorId = authorId;
        }
    }
}