using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.QueryHandlers
{
    internal class IsAuthorAddedByHandler : IQueryHandler<IsAuthorAddedByQuery, bool>
    {
        private readonly PublicationsContext _context;

        public IsAuthorAddedByHandler(PublicationsContext context)
        {
            this._context = context;
        }

        public Task<bool> HandleAsync(IsAuthorAddedByQuery query)
        {
            return this._context.Authors
                .Include(p => p.AddedBy)
                .AsNoTracking()
                .AnyAsync(p => p.Guid == query.AuthorGuid
                               && p.AddedBy.ReaderGuid == query.ReaderGuid);
        }
    }
}