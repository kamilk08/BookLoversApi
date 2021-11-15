using System;
using System.Linq;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Services
{
    internal class PublisherCycleUniquenessChecker : IPublisherCycleUniquenessChecker
    {
        private readonly PublicationsContext _context;

        public PublisherCycleUniquenessChecker(PublicationsContext context)
        {
            this._context = context;
        }

        public bool IsUnique(Guid guid)
        {
            return !this._context.PublisherCycles.AsNoTracking()
                .Any(a => a.Guid == guid);
        }
    }
}