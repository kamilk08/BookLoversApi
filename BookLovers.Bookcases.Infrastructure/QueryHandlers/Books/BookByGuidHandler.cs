using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;
using BookLovers.Bookcases.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.QueryHandlers.Books
{
    internal class BookByGuidHandler : IQueryHandler<BookByGuidQuery, BookDto>
    {
        private readonly BookcaseContext _context;
        private readonly IMapper _mapper;

        public BookByGuidHandler(BookcaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookDto> HandleAsync(BookByGuidQuery query)
        {
            var book = await _context.Books.AsNoTracking()
                .SingleOrDefaultAsync(p => p.BookGuid == query.BookGuid);

            return _mapper.Map<BookReadModel, BookDto>(book);
        }
    }
}