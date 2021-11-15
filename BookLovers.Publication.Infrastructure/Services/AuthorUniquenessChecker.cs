using System;
using System.Linq;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Services
{
    internal class AuthorUniquenessChecker : IAuthorUniquenessChecker
    {
        private readonly PublicationsContext _context;

        public AuthorUniquenessChecker(PublicationsContext context)
        {
            this._context = context;
        }

        public bool IsUnique(Guid guid)
        {
            return !this._context.Authors.AsNoTracking()
                .Any(a => a.Guid == guid);
        }
    }
}