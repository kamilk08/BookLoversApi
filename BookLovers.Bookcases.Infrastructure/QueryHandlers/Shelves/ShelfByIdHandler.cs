using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries.Shelves;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Shelves
{
    internal class ShelfByIdHandler : IQueryHandler<ShelfByIdQuery, ShelfDto>
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public ShelfByIdHandler(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ShelfDto> HandleAsync(ShelfByIdQuery query)
        {
            var shelf = await _context.Shelves.AsNoTracking()
                .Include(p => p.Books)
                .Include(p => p.Bookcase)
                .SingleOrDefaultAsync(p => p.Id == query.ShelfId);

            return _mapper.Map<ShelfDto>(shelf);
        }
    }
}