using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Authors
{
    public class MultipleAuthorsQuery : IQuery<IList<AuthorDto>>
    {
        public int[] Ids { get; set; }

        public MultipleAuthorsQuery()
        {
            this.Ids = this.Ids ?? new int[0];
        }

        public MultipleAuthorsQuery(int[] ids)
        {
            this.Ids = ids;
        }
    }
}