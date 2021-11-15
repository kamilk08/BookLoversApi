using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Queries.Readers;

namespace BookLovers.Ratings.Infrastructure.QueryHandlers.Readers
{
    internal class ReaderRatingsHandler : IQueryHandler<ReaderRatingsQuery, ReaderRatingsDto>
    {
        private readonly RatingsContext _context;

        public ReaderRatingsHandler(RatingsContext context)
        {
            this._context = context;
        }

        public async Task<ReaderRatingsDto> HandleAsync(ReaderRatingsQuery query)
        {
            var gropedRatings = await this._context.Books.AsNoTracking()
                .Include(p => p.Ratings)
                .Where(s => s.Ratings.Any(a => a.ReaderId == query.ReaderId))
                .SelectMany(sm => sm.Ratings)
                .Where(p => p.ReaderId == query.ReaderId)
                .GroupBy(p => p.Stars)
                .ToDictionaryAsync(k => k.Key, v => v.Count());

            return new ReaderRatingsDto()
            {
                ReaderId = query.ReaderId,
                RatingsCount = gropedRatings.Values.Sum(),
                GropedRatings = gropedRatings
            };
        }
    }
}