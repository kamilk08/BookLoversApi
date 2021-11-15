using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Books
{
    public class MultipleBooksByGuidQuery : IQuery<List<BookDto>>
    {
        public List<Guid> Guides { get; set; }

        public MultipleBooksByGuidQuery()
        {
            this.Guides = this.Guides ?? new List<Guid>();
        }

        public MultipleBooksByGuidQuery(List<Guid> guides)
        {
            this.Guides = guides;
        }
    }
}