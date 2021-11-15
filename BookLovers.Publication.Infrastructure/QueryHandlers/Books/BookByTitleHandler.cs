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
    internal class BookByTitleHandler : IQueryHandler<BookByTitleQuery, BookDto>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public BookByTitleHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<BookDto> HandleAsync(BookByTitleQuery query)
        {
            var bookQuery = _context.Books.Include(p => p.Authors)
                .AsNoTracking()
                .ActiveRecords()
                .WithExactTitle(query.Title).FutureValue();

            var cycleQuery = _context.PublisherCycles.AsNoTracking()
                .Include(p => p.CycleBooks)
                .Include(p => p.Publisher)
                .ActiveRecords()
                .Where(p => p.CycleBooks.Any(b => b.Title.ToUpper() == query.Title.ToUpper()))
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