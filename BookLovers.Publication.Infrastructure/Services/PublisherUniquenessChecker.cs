using System;
using System.Linq;
using BookLovers.Publication.Domain.Publishers.Services;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Services
{
    internal class PublisherUniquenessChecker : IPublisherUniquenessChecker
    {
        private readonly PublicationsContext _context;

        public PublisherUniquenessChecker(PublicationsContext context)
        {
            this._context = context;
        }

        public bool IsUnique(Guid guid, string name)
        {
            return !this._context.Publishers.AsNoTracking()
                .Any(a => a.Guid == guid || a.Publisher == name);
        }
    }
}