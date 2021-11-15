using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Bookcases
{
    internal class BookcaseOptionsByIdHandler :
        IQueryHandler<BookcaseOptionsByIdQuery, BookcaseOptionsDto>
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public BookcaseOptionsByIdHandler(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<BookcaseOptionsDto> HandleAsync(BookcaseOptionsByIdQuery query)
        {
            return _context.SettingsManagers
                .AsNoTracking()
                .Where(p => p.BookcaseId == query.BookcaseId)
                .Select(s => new BookcaseOptionsDto
                {
                    Capacity = s.Capacity,
                    Privacy = s.Privacy
                }).SingleOrDefaultAsync();
        }
    }
}