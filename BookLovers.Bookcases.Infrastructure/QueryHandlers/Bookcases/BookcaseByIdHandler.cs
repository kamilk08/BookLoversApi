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
    internal class BookcaseByIdHandler : IQueryHandler<BookcaseByIdQuery, BookcaseDto>
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public BookcaseByIdHandler(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookcaseDto> HandleAsync(BookcaseByIdQuery query)
        {
            var bookcaseQuery = _context.Bookcases
                .Include(p => p.Shelves.Select(s => s.Books)).AsNoTracking()
                .ActiveRecords()
                .Where(p => p.Id == query.BookcaseId)
                .FutureValue();

            var settingsQuery = _context.SettingsManagers.Where(p => p.BookcaseId == query.BookcaseId).FutureValue();

            var bookcase = await bookcaseQuery.ValueAsync();
            var settings = await settingsQuery.ValueAsync();

            if (bookcase == null)
                return null;

            foreach (var shelf in bookcase.Shelves)
            {
                var filteredShelf = shelf.Books.Where(a => a.Status == AggregateStatus.Active.Value).ToList();
                shelf.Books = filteredShelf;
            }

            var bookcaseDto = _mapper.Map<BookcaseReadModel, BookcaseDto>(bookcase);
            bookcaseDto.BookcaseOptions = _mapper.Map<SettingsManagerReadModel, BookcaseOptionsDto>(settings);

            return bookcaseDto;
        }
    }
}