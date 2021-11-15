using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;
using BookLovers.Publication.Infrastructure.Queries.Authors;
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Authors
{
    internal class FindAuthorHandler : IQueryHandler<PaginatedAuthorsQuery, PaginatedResult<AuthorDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public FindAuthorHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<AuthorDto>> HandleAsync(
            PaginatedAuthorsQuery query)
        {
            var baseQuery = this._context.Authors.AsNoTracking()
                .Include(p => p.AuthorFollowers)
                .Include(p => p.AuthorBooks)
                .Include(p => p.SubCategories)
                .Include(p => p.Quotes)
                .Include(p => p.AddedBy)
                .WithAuthorFullName(query.Value);

            var totalCountQuery = baseQuery.DeferredCount();
            var resultsQuery = baseQuery.OrderBy(p => p.Id)
                .Paginate(query.Page, query.Count);

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await resultsQuery.ToListAsync();

            var mappedResults = this._mapper.Map<List<AuthorReadModel>, List<AuthorDto>>(results);

            var paginatedResult = new PaginatedResult<AuthorDto>(mappedResults, query.Page, query.Count, totalCount);

            return paginatedResult;
        }
    }
}