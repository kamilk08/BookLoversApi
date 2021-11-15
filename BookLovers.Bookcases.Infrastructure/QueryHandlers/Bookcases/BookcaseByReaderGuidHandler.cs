using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Bookcases
{
    internal class BookcaseByReaderGuidHandler : IQueryHandler<BookcaseByReaderGuidQuery, BookcaseDto>
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public BookcaseByReaderGuidHandler(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookcaseDto> HandleAsync(BookcaseByReaderGuidQuery query)
        {
            var bookcase = await _context.Bookcases.AsNoTracking()
                .Include(p => p.Shelves.Select(s => s.Books))
                .SingleOrDefaultAsync(p => p.ReaderGuid == query.ReaderGuid);

            if (bookcase == null)
                return null;

            var manager = await _context.SettingsManagers.SingleOrDefaultAsync(p => p.BookcaseGuid == bookcase.Guid);

            foreach (var shelf in bookcase.Shelves)
            {
                var filteredShelf = shelf.Books.Where(a => a.Status == AggregateStatus.Active.Value).ToList();
                shelf.Books = filteredShelf;
            }

            var bookcaseDto = _mapper.Map<BookcaseReadModel, BookcaseDto>(bookcase);
            bookcaseDto.BookcaseOptions = _mapper.Map<SettingsManagerReadModel, BookcaseOptionsDto>(manager);

            return bookcaseDto;
        }
    }
}