using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Infrastructure.Queries.FilteringExtensions;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Services.BookReviewsSortingServices
{
    internal class LikesReviewsSorter : IReviewsSorter
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public ReviewsSortingType SortingType => ReviewsSortingType.ByLikes;

        public LikesReviewsSorter(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<ReviewDto>> Sort(
            PaginatedBookReviewsQuery query)
        {
            var baseQuery = this._context.Reviews
                .Include(p => p.Reader)
                .Include(p => p.Book)
                .Include(p => p.SpoilerTags)
                .Include(p => p.ReviewReports)
                .Include(p => p.Likes)
                .AsNoTracking().ActiveRecords()
                .Where(p => p.Book.BookId == query.BookId)
                .WithContent();

            var totalCountQuery = baseQuery.DeferredCount();
            IOrderedQueryable<ReviewReadModel> orderedQuery;

            if (!query.Descending)
                orderedQuery = baseQuery.OrderBy(p => p.LikesCount);
            else
                orderedQuery = baseQuery.OrderByDescending(p => p.LikesCount);

            var paginatedReviewsQuery = orderedQuery.Paginate(query.Page, query.Count).Future();

            var totalReviews = await totalCountQuery.ExecuteAsync();
            var results = await paginatedReviewsQuery.ToListAsync();
            var mappedResults = this._mapper.Map<List<ReviewReadModel>, List<ReviewDto>>(results);

            var paginatedResult = new PaginatedResult<ReviewDto>(mappedResults, query.Page, query.Count, totalReviews);

            return paginatedResult;
        }
    }
}