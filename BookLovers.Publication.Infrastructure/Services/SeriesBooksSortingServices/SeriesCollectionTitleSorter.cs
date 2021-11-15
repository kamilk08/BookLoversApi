﻿using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;
using BookLovers.Publication.Infrastructure.Queries.Series;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.Services.SeriesBooksSortingServices
{
    internal class SeriesCollectionTitleSorter : ISeriesCollectionSorter
    {
        private readonly PublicationsContext _context;

        public SeriesCollectionSortingType SortingType => SeriesCollectionSortingType.ByTitle;

        public SeriesCollectionTitleSorter(PublicationsContext context)
        {
            this._context = context;
        }

        public async Task<PaginatedResult<int>> Sort(
            PaginatedSeriesCollectionQuery query)
        {
            var baseQuery = this._context.Books
                .Include(p => p.Series)
                .Where(p => p.Series.Id == query.SeriesId)
                .FilterBooksByTitle(query.Title);

            var totalCountQuery = baseQuery.DeferredCount();
            IOrderedQueryable<BookReadModel> orderedQuery;

            if (!query.Descending)
                orderedQuery = baseQuery.OrderByDescending(p => p.Title);
            else
                orderedQuery = baseQuery.OrderBy(p => p.Title);

            var resultsQuery = orderedQuery
                .Paginate(query.Page, query.Count)
                .Select(s => s.Id).Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await resultsQuery.ToListAsync();

            var paginatedResult = new PaginatedResult<int>(results, query.Page, query.Count,
                totalCount);

            return paginatedResult;
        }
    }
}