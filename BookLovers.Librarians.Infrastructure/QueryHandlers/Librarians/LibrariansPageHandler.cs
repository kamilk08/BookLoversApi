using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Queries.Librarians;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace BookLovers.Librarians.Infrastructure.QueryHandlers.Librarians
{
    internal class LibrariansPageHandler :
        IQueryHandler<LibrarianPageQuery, PaginatedResult<LibrarianDto>>
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public LibrariansPageHandler(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<LibrarianDto>> HandleAsync(
            LibrarianPageQuery query)
        {
            var baseQuery = _context
                .PromotionWaiters
                .AsNoTracking()
                .Where(p => query.Ids.Any(a => a == p.ReaderId))
                .Join(_context.Librarians.ActiveRecords(), (t => t.ReaderGuid), (l => l.ReaderGuid), (t, l) =>
                    new LibrarianDto
                    {
                        Id = l.Id,
                        Guid = l.Guid,
                        ReaderId = t.ReaderId
                    });

            var totalItemsQuery = baseQuery.DeferredCount();
            var itemsQuery = baseQuery.Future();

            var totalItems = await totalItemsQuery.ExecuteAsync();
            var items = await itemsQuery.ToListAsync();

            return new PaginatedResult<LibrarianDto>(items, query.Page, query.Count, totalItems);
        }
    }
}