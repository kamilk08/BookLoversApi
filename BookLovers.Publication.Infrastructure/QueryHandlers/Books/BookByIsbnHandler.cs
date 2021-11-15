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
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;
using Z.EntityFramework.Plus;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Books
{
    internal class BookByIsbnHandler : IQueryHandler<BookByIsbnQuery, BookDto>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public BookByIsbnHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<BookDto> HandleAsync(BookByIsbnQuery query)
        {
            var bookQuery = _context.Books.Include(p => p.Publisher)
                .Include(p => p.Authors)
                .AsNoTracking()
                .ActiveRecords()
                .WithExactIsbn(query.Isbn)
                .FutureValue();

            var cycleQuery = _context.PublisherCycles.AsNoTracking()
                .Include(p => p.CycleBooks)
                .Include(p => p.Publisher)
                .ActiveRecords()
                .Where(p => p.CycleBooks.Any(b => b.Isbn == query.Isbn))
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