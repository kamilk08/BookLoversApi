using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Authors
{
    public class MultipleAuthorsByGuidQuery : IQuery<IList<AuthorDto>>
    {
        public List<Guid> Guides { get; set; }

        public MultipleAuthorsByGuidQuery()
        {
            this.Guides = this.Guides ?? new List<Guid>();
        }

        public MultipleAuthorsByGuidQuery(List<Guid> guides)
        {
            this.Guides = guides;
        }
    }
}