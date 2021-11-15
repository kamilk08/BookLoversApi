using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Authors
{
    public class AuthorByGuidQuery : IQuery<AuthorDto>
    {
        public Guid AuthorGuid { get; set; }

        public AuthorByGuidQuery()
        {
        }

        public AuthorByGuidQuery(Guid authorGuid)
        {
            this.AuthorGuid = authorGuid;
        }
    }
}