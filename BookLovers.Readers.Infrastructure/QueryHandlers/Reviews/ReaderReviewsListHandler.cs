using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Reviews
{
    internal class ReaderReviewsListHandler :
        IQueryHandler<ReaderReviewsListQuery, PaginatedResult<ReviewDto>>
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public ReaderReviewsListHandler(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<ReviewDto>> HandleAsync(
            ReaderReviewsListQuery query)
        {
            var baseQuery = this._context.Reviews.AsNoTracking()
                .Include(p => p.Reader).Include(p => p.Book)
                .Include(p => p.SpoilerTags).Include(p => p.ReviewReports)
                .Include(p => p.Likes).ActiveRecords()
                .Where(p => p.Reader.ReaderId == query.ReaderId);

            var totalCountQuery = baseQuery.DeferredCount();

            var resultsQuery = baseQuery.OrderByDescending(p => p.CreateDate)
                .Paginate(query.Page, query.Count).Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await resultsQuery.ToListAsync();

            var mappedResults = this._mapper.Map<List<ReviewReadModel>, List<ReviewDto>>(results);

            var paginatedResult = results != null
                ? new PaginatedResult<ReviewDto>(
                    mappedResults, query.Page, query.Count, totalCount)
                : new PaginatedResult<ReviewDto>(query.Page);

            return paginatedResult;
        }
    }
}