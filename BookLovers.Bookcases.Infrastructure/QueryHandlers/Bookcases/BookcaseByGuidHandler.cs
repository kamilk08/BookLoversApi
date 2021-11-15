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
    internal class BookcaseByGuidHandler : IQueryHandler<BookcaseByGuidQuery, BookcaseDto>
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public BookcaseByGuidHandler(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookcaseDto> HandleAsync(BookcaseByGuidQuery query)
        {
            var settingsManagerQuery = _context.SettingsManagers.AsNoTracking()
                .Where(p => p.BookcaseGuid == query.BookcaseGuid)
                .FutureValue();

            var bookcaseQuery = _context.Bookcases.AsNoTracking()
                .Include(p => p.Shelves.Select(s => s.Books))
                .Where(p => p.Guid == query.BookcaseGuid)
                .FutureValue();

            var options = await settingsManagerQuery.ValueAsync();
            var bookcase = await bookcaseQuery.ValueAsync();

            if (bookcase == null)
                return null;

            foreach (var shelf in bookcase.Shelves)
            {
                var filteredShelf = shelf.Books.Where(a => a.Status == AggregateStatus.Active.Value).ToList();
                shelf.Books = filteredShelf;
            }

            var bookcaseDto = _mapper.Map<BookcaseReadModel, BookcaseDto>(bookcase);
            bookcaseDto.BookcaseOptions = _mapper.Map<SettingsManagerReadModel, BookcaseOptionsDto>(options);

            return bookcaseDto;
        }
    }
}