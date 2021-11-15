using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries.Shelves;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Shelves
{
    internal class ShelfByGuidHandler : IQueryHandler<ShelfByGuidQuery, ShelfDto>
    {
        private readonly BookcaseContext _readContext;
        private readonly IMapper _mapper;

        public ShelfByGuidHandler(BookcaseContext readContext, IMapper mapper)
        {
            _readContext = readContext;
            _mapper = mapper;
        }

        public async Task<ShelfDto> HandleAsync(ShelfByGuidQuery query)
        {
            var shelf = await _readContext.Shelves.AsNoTracking()
                .Include(p => p.Books)
                .Include(p => p.Bookcase)
                .SingleOrDefaultAsync(p => p.Guid == query.ShelfGuid);

            return _mapper.Map<ShelfDto>(shelf);
        }
    }
}