using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Ratings.Domain.Authors
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<Author> GetByIdAsync(int authorId);

        Task<Author> GetByAuthorGuidAsync(Guid authorGuid);

        Task<IEnumerable<Author>> GetMultipleAuthorsAsync(
            IEnumerable<Guid> authorGuides);

        Task<IEnumerable<Author>> GetAuthorsWithBookAsync(Guid bookGuid);
    }
}