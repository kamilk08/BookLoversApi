using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries
{
    public class BookSearchQuery : IQuery<PaginatedResult<BookDto>>
    {
        public IList<int> Categories { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string ISBN { get; set; }

        public DateTime? From { get; set; }

        public DateTime? Till { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public BookSearchQuery(BookFilterCriteria criteria)
        {
            this.Categories = criteria.Categories ?? new List<int>();
            this.Title = criteria.Title ?? string.Empty;
            this.Author = criteria.Author ?? string.Empty;
            this.ISBN = criteria.ISBN ?? string.Empty;
            this.From = criteria.From ?? null;
            this.Till = criteria.Till ?? null;
            this.Page = criteria.Page;
            this.Count = criteria.Count;
        }
    }
}