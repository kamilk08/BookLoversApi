using System;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Root.Domain
{
    internal class UnknownAuthorService : IUnknownAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PublicationsContext _context;

        public UnknownAuthorService(IUnitOfWork unitOfWork, PublicationsContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<Author> GetUnknownAuthorAsync()
        {
            var unknownAuthor = _context.Authors.AsNoTracking()
                .Single(p => p.FullName == UnknownAuthor.Key);

            return await _unitOfWork.GetAsync<Author>(unknownAuthor.Guid);
        }

        public async Task AddBookToUnknownAuthorAsync(Guid bookGuid)
        {
            var unknownAuthor = await GetUnknownAuthorAsync();

            unknownAuthor.AddBook(new AuthorBook(bookGuid));

            await _unitOfWork.CommitAsync(unknownAuthor);
        }
    }
}