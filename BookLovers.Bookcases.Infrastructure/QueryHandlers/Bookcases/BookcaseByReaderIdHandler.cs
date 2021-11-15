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
using Z.EntityFramework.Plus;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Bookcases
{
    internal class BookcaseByReaderIdHandler : IQueryHandler<BookcaseByReaderIdQuery, BookcaseDto>
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public BookcaseByReaderIdHandler(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookcaseDto> HandleAsync(BookcaseByReaderIdQuery query)
        {
            var bookcaseQuery = _context.Bookcases.AsNoTracking()
                .Include(p => p.Shelves.Select(s => s.Books))
                .Where(p => p.ReaderId == query.ReaderId)
                .FutureValue();

            var bookcase = await bookcaseQuery.ValueAsync();
            if (bookcase == null)
                return null;

            foreach (var shelf in bookcase.Shelves)
            {
                var filteredShelf = shelf.Books.Where(a => a.Status == AggregateStatus.Active.Value).ToList();
                shelf.Books = filteredShelf;
            }

            var settingsManagers = await _context.SettingsManagers
                .SingleOrDefaultAsync(p => p.BookcaseGuid == bookcase.Guid);

            var bookcaseDto = _mapper.Map<BookcaseReadModel, BookcaseDto>(bookcase);
            bookcaseDto.BookcaseOptions = _mapper.Map<SettingsManagerReadModel, BookcaseOptionsDto>(settingsManagers);

            return bookcaseDto;
        }
    }
}