using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;
using System;

namespace BookLovers.Librarians.Infrastructure.Queries.Librarians
{
    public class LibrarianByGuidQuery : IQuery<LibrarianDto>
    {
        public Guid LibrarianGuid { get; }

        public LibrarianByGuidQuery(Guid librarianGuid)
        {
            this.LibrarianGuid = librarianGuid;
        }
    }
}