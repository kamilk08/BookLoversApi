using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries.FilteringExtensions;
using BookLovers.Bookcases.Infrastructure.Queries.Shelves;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Shelves
{
    internal class BookOnShelvesHandler :
        IQueryHandler<BookOnShelvesQuery, PaginatedResult<KeyValuePair<string, int>>>
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public BookOnShelvesHandler(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<KeyValuePair<string, int>>> HandleAsync(
            BookOnShelvesQuery query)
        {
            var source = _context.Shelves.Include(p => p.Books)
                .AsNoTracking()
                .AllShelvesThatHaveBook(query.BookId)
                .GroupBy(shelf => shelf.ShelfName)
                .Select(s => new
                {
                    Shelf = s.Key,
                    Count = s.Count()
                });

            int totalCount = source.Count();

            var dict = await source.OrderByDescending(p => p.Count)
                .Paginate(query.Page, query.Count)
                .ToDictionaryAsync(k => k.Shelf, v => v.Count);

            var results = dict.Select(s => new KeyValuePair<string, int>(s.Key, s.Value)).ToList();

            return new PaginatedResult<KeyValuePair<string, int>>(results, query.Page, query.Count, totalCount);
        }
    }
}