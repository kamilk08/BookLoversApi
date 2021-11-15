using System;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Domain.Librarians
{
    public interface ILibrarianRepository
    {
        Task<Librarian> GetLibrarianByReaderGuid(Guid readerGuid);
    }
}