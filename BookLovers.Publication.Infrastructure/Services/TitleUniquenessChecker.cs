using System.Linq;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Domain.Books.Services;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Services
{
    internal class TitleUniquenessChecker : ITitleUniquenessChecker
    {
        private readonly PublicationsContext _context;

        public TitleUniquenessChecker(PublicationsContext context)
        {
            this._context = context;
        }

        public bool IsUnique(string title)
        {
            return !this._context.Books.ActiveRecords().Any(p => p.Title == title);
        }
    }
}