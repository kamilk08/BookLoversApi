using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions
{
    internal static class PublisherCycleFilterExtensions
    {
        internal static IQueryable<PublisherCycleReadModel> FilterCyclesByTitle(
            this IQueryable<PublisherCycleReadModel> query, string title)
        {
            return query.WhereIf(p => p.Cycle.ToUpper().StartsWith(title.ToUpper()), !title.IsEmpty());
        }

        internal static Task<PublisherCycleReadModel> FindCycleWithExactName(
            this IQueryable<PublisherCycleReadModel> query, string cycleName)
        {
            return query.SingleOrDefaultAsync(p => p.Cycle.ToUpper() == cycleName.Trim().ToUpper());
        }
    }
}