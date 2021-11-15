using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries.Shelves;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Shelves
{
    internal class ShelfByNameHandler : IQueryHandler<ShelfByNameQuery, ShelfDto>
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public ShelfByNameHandler(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ShelfDto> HandleAsync(ShelfByNameQuery query)
        {
            var shelf = await _context.Shelves
                .AsNoTracking()
                .Include(p => p.Bookcase)
                .Include(p => p.Books)
                .SingleOrDefaultAsync(p => p.ShelfName == query.ShelfName && p.Bookcase.Id == query.BookcaseId);

            return _mapper.Map<ShelfDto>(shelf);
        }
    }
}