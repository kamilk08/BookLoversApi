using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using BookLovers.Bookcases.Infrastructure.Queries.FilteringExtensions;
using BookLovers.Bookcases.Infrastructure.Queries.Shelves;
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Shelves
{
    internal class PaginatedReaderShelvesHandler :
        IQueryHandler<PaginatedReaderShelvesQuery, PaginatedResult<ShelfDto>>
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public PaginatedReaderShelvesHandler(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<ShelfDto>> HandleAsync(
            PaginatedReaderShelvesQuery query)
        {
            var baseQuery = _context.Shelves
                .AsNoTracking()
                .Include(p => p.Books)
                .Include(p => p.Bookcase)
                .WithBookcase(query.BookcaseId);

            var countQuery = baseQuery.DeferredCount().FutureValue();
            var shelvesQuery = baseQuery.OrderBy(p => p.Id)
                .Paginate(query.Page, query.Count).Future();

            int totalCount = await countQuery.ValueAsync();

            var results = await shelvesQuery.ToListAsync();
            if (results == null)
                return new PaginatedResult<ShelfDto>(query.Page);

            var mappedResults = _mapper.Map<List<ShelfReadModel>, List<ShelfDto>>(results);

            return new PaginatedResult<ShelfDto>(mappedResults, query.Page, query.Count, totalCount);
        }
    }
}