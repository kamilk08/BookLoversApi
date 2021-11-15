using System;
using System.Linq;
using BookLovers.Publication.Domain.SeriesCycle.Services;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Services
{
    internal class SeriesUniquenessChecker : ISeriesUniquenessChecker
    {
        private readonly PublicationsContext _context;

        public SeriesUniquenessChecker(PublicationsContext context)
        {
            this._context = context;
        }

        public bool IsUnique(Guid guid, string name)
        {
            return !this._context.Series.AsNoTracking()
                .Any(a => a.Guid == guid || a.Name == name);
        }
    }
}