using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;

namespace BookLovers.Librarians.Infrastructure.Queries.Librarians
{
    public class LibrarianByIdQuery : IQuery<LibrarianDto>
    {
        public int LibrarianId { get; set; }

        public LibrarianByIdQuery()
        {
        }

        public LibrarianByIdQuery(int librarianId)
        {
            this.LibrarianId = librarianId;
        }
    }
}