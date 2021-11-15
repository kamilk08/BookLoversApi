using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using BookLovers.Bookcases.Infrastructure.Queries.Shelves;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Shelves
{
    internal class ShelfRecordHandler : IQueryHandler<ShelfRecordQuery, ShelfRecordDto>
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public ShelfRecordHandler(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ShelfRecordDto> HandleAsync(ShelfRecordQuery query)
        {
            var shelfRecord = await _context.BookOnShelvesRecords
                .AsNoTracking()
                .Include(p => p.Book)
                .Include(p => p.Shelf)
                .SingleOrDefaultAsync(p => p.BookId == query.BookId
                                           && p.ShelfId == query.ShelfId);

            return _mapper.Map<ShelfRecordReadModel, ShelfRecordDto>(shelfRecord);
        }
    }
}