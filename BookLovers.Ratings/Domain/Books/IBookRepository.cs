using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Ratings.Domain.Books
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book> GetByBookGuidAsync(Guid bookGuid);

        Task<Book> GetByIdAsync(int bookId);
    }
}