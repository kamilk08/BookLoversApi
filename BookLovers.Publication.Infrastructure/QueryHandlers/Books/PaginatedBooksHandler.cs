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
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Books
{
    internal class PaginatedBooksHandler : IQueryHandler<PaginatedBooksQuery, PaginatedResult<BookDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public PaginatedBooksHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<BookDto>> HandleAsync(
            PaginatedBooksQuery query)
        {
            var booksQuery = _context.Books
                .Include(p => p.Authors)
                .Include(p => p.Publisher)
                .Include(p => p.Series)
                .Include(p => p.Reviews)
                .Include(p => p.Reader)
                .Include(p => p.Quotes)
                .AsNoTracking()
                .ActiveRecords();

            var totalCountQuery = booksQuery.DeferredCount();
            var bookResultsQuery = booksQuery.OrderBy(p => p.Id)
                .Paginate(query.Page, query.Count)
                .Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var bookResults = await bookResultsQuery.ToListAsync();

            var mappedBooks = _mapper.Map<IList<BookReadModel>, IList<BookDto>>(bookResults);

            return new PaginatedResult<BookDto>(mappedBooks, query.Page, query.Count, totalCount);
        }
    }
}