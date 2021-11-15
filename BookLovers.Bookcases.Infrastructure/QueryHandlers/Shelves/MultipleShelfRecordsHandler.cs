using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries.FilteringExtensions;
using BookLovers.Bookcases.Infrastructure.Queries.Shelves;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Shelves
{
    internal class MultipleShelfRecordsHandler :
        IQueryHandler<MultipleShelfRecordsQuery, List<ShelfRecordDto>>
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public MultipleShelfRecordsHandler(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ShelfRecordDto>> HandleAsync(
            MultipleShelfRecordsQuery query)
        {
            var results = await _context.BookOnShelvesRecords
                .Include(p => p.Book)
                .Include(p => p.Shelf.Bookcase)
                .WithoutCustomShelves()
                .WithBookcase(query.BookcaseId)
                .WithBooksOnShelves(query.BookIds)
                .Distinct()
                .ToListAsync();

            return _mapper.Map<List<ShelfRecordDto>>(results);
        }
    }
}