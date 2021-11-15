using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using BookLovers.Publication.Infrastructure.Queries.Books;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Books
{
    internal class BookByGuidHandler : IQueryHandler<BookByGuidQuery, BookDto>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public BookByGuidHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<BookDto> HandleAsync(BookByGuidQuery query)
        {
            var bookQuery = _context.Books
                .Include(p => p.Authors)
                .Include(p => p.Publisher)
                .Include(p => p.Series)
                .Include(p => p.Reviews)
                .Include(p => p.Reader)
                .Include(p => p.Quotes)
                .ActiveRecords()
                .Where(p => p.Guid == query.BookGuid)
                .FutureValue();

            var cycleQuery = _context.PublisherCycles.AsNoTracking()
                .Include(p => p.CycleBooks)
                .Include(p => p.Publisher)
                .ActiveRecords()
                .Where(p => p.CycleBooks.Any(b => b.Guid == query.BookGuid))
                .Future();

            var book = await bookQuery.ValueAsync();
            var cycle = await cycleQuery.ToListAsync();

            BookDto dto = null;

            if (book != null)
            {
                dto = _mapper.Map<BookReadModel, BookDto>(book);
                dto.Cycles = _mapper.Map<IEnumerable<PublisherCycleReadModel>, IEnumerable<PublisherCycleDto>>(cycle);
            }

            return dto;
        }
    }
}