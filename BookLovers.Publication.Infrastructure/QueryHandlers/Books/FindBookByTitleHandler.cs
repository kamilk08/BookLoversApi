using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using BookLovers.Publication.Infrastructure.Queries.Books;
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Books
{
    internal class FindBookByTitleHandler :
        IQueryHandler<FindBookByTitleQuery, PaginatedResult<BookDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public FindBookByTitleHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<BookDto>> HandleAsync(
            FindBookByTitleQuery query)
        {
            var booksQuery = _context.Books
                .Include(p => p.Authors)
                .Include(p => p.Publisher)
                .Include(p => p.Series)
                .Include(p => p.Reviews)
                .Include(p => p.Reader)
                .Include(p => p.Quotes)
                .AsNoTracking()
                .ActiveRecords()
                .FilterBooksByTitle(query.Value);

            var totalCountQuery = booksQuery.DeferredCount();
            var paginatedResultsQuery = booksQuery.OrderBy(p => p.Id)
                .Paginate(query.Page, query.Count).Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var paginatedResult = await paginatedResultsQuery.ToListAsync();

            var mappedResults = _mapper.Map<List<BookReadModel>, List<BookDto>>(paginatedResult);

            return new PaginatedResult<BookDto>(mappedResults, query.Page, query.Count, totalCount);
        }
    }
}