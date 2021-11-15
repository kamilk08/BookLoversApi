using System;
using System.Threading.Tasks;

namespace BookLovers.Publication.Domain.Authors.Services
{
    public interface IUnknownAuthorService
    {
        Task<Author> GetUnknownAuthorAsync();

        Task AddBookToUnknownAuthorAsync(Guid bookGuid);
    }
}