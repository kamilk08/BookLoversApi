using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Reviews;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;
using BookLovers.Readers.Infrastructure.Services.BookReviewsSortingServices;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Reviews
{
    internal class PaginatedBookReviewsHandler :
        IQueryHandler<PaginatedBookReviewsQuery, PaginatedResult<ReviewDto>>
    {
        private readonly IDictionary<ReviewsSortingType, IReviewsSorter> _sorters;

        public PaginatedBookReviewsHandler(
            IDictionary<ReviewsSortingType, IReviewsSorter> sorters)
        {
            this._sorters = sorters;
        }

        public Task<PaginatedResult<ReviewDto>> HandleAsync(
            PaginatedBookReviewsQuery query)
        {
            var reviewsSorter = this._sorters.Values.SingleOrDefault(p => p.SortingType.Value == query.SortType);

            return reviewsSorter == null
                ? Task.FromResult(new PaginatedResult<ReviewDto>(query.Page))
                : reviewsSorter.Sort(query);
        }
    }
}