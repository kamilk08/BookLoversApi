using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using BookLovers.Publication.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Books
{
    internal class BookSearchHandler : IQueryHandler<BookSearchQuery, PaginatedResult<BookDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public BookSearchHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<BookDto>> HandleAsync(
            BookSearchQuery query)
        {
            var baseQuery = this._context.Books
                .Include(p => p.Authors)
                .Include(p => p.Publisher)
                .Include(p => p.Series)
                .Include(p => p.Reviews)
                .Include(p => p.Reader)
                .Include(p => p.Quotes)
                .AsNoTracking()
                .ActiveRecords()
                .FilterBooksByTitle(query.Title)
                .FilterBooksByAuthorFullName(query.Author)
                .FilterBooksByIsbn(query.ISBN)
                .FilterFromDate(query.From)
                .FilterTillDate(query.Till)
                .FilterWithCategories(query.Categories);

            var totalResultsQuery = baseQuery.DeferredCount();
            var paginatedResultsQuery = baseQuery.OrderBy(p => p.Id)
                .Paginate(query.Page, query.Count)
                .Future();

            var totalResults = await totalResultsQuery.ExecuteAsync();
            var paginatedResults = await paginatedResultsQuery.ToListAsync();
            if (totalResults == 0)
                return new PaginatedResult<BookDto>(query.Page);

            var mappedResults = _mapper.Map<List<BookReadModel>, List<BookDto>>(paginatedResults);

            return new PaginatedResult<BookDto>(mappedResults, query.Page,
                query.Count, totalResults);
        }
    }
}