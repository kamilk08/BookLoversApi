using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos;

namespace BookLovers.Readers.Infrastructure.Queries
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