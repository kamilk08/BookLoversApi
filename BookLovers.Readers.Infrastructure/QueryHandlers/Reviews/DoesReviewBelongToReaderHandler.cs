using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Reviews
{
    internal class DoesReviewBelongToReaderHandler : IQueryHandler<DoesReviewBelongToReaderQuery, bool>
    {
        private readonly ReadersContext _context;

        public DoesReviewBelongToReaderHandler(ReadersContext context)
        {
            this._context = context;
        }

        public Task<bool> HandleAsync(DoesReviewBelongToReaderQuery query)
        {
            return this._context.Reviews
                .Include(p => p.Reader)
                .AsNoTracking()
                .ActiveRecords()
                .AnyAsync(p => p.Guid == query.ReviewGuid
                               && p.Reader.Guid == query.ReaderGuid);
        }
    }
}