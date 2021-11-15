using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.QueryHandlers
{
    internal class IsBookAddedByHandler : IQueryHandler<IsBookAddedByQuery, bool>
    {
        private readonly PublicationsContext _context;

        public IsBookAddedByHandler(PublicationsContext context)
        {
            this._context = context;
        }

        public Task<bool> HandleAsync(IsBookAddedByQuery query)
        {
            return this._context.Books.AsNoTracking().Include(p => p.Reader)
                .AnyAsync(p => p.Guid == query.BookGuid
                               && p.Reader.ReaderGuid == query.ReaderGuid);
        }
    }
}