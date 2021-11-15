using System.Linq;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Domain.Books.Services;
using BookLovers.Publication.Infrastructure.Persistence;

namespace BookLovers.Publication.Infrastructure.Services
{
    public class IsbnUniquenessChecker : IIsbnUniquenessChecker
    {
        private readonly PublicationsContext _context;

        public IsbnUniquenessChecker(PublicationsContext context)
        {
            this._context = context;
        }

        public bool IsUnique(string isbn)
        {
            return !this._context.Books.ActiveRecords().Any(a => a.Isbn == isbn);
        }
    }
}