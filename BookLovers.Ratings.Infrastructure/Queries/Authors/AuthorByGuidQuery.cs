using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Authors
{
    public class AuthorByGuidQuery : IQuery<AuthorDto>
    {
        public Guid AuthorGuid { get; }

        public AuthorByGuidQuery(Guid authorGuid)
        {
            this.AuthorGuid = authorGuid;
        }
    }
}