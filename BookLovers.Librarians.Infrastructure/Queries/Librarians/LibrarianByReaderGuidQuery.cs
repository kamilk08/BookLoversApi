using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;
using System;

namespace BookLovers.Librarians.Infrastructure.Queries.Librarians
{
    public class LibrarianByReaderGuidQuery : IQuery<LibrarianDto>
    {
        public Guid Guid { get; set; }

        public LibrarianByReaderGuidQuery()
        {
        }

        public LibrarianByReaderGuidQuery(Guid guid)
        {
            this.Guid = guid;
        }
    }
}