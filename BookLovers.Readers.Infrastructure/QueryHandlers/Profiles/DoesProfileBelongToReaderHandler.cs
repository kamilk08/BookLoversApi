using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.Readers.Profiles;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Profiles
{
    internal class DoesProfileBelongToReaderHandler :
        IQueryHandler<DoesProfileBelongToReaderQuery, bool>
    {
        private readonly ReadersContext _context;

        public DoesProfileBelongToReaderHandler(ReadersContext context)
        {
            this._context = context;
        }

        public Task<bool> HandleAsync(DoesProfileBelongToReaderQuery query)
        {
            return this._context.Profiles
                .Include(p => p.Reader).AsNoTracking()
                .AnyAsync(p => p.Guid == query.ProfileGuid
                               && p.Reader.Guid == query.ReaderGuid);
        }
    }
}