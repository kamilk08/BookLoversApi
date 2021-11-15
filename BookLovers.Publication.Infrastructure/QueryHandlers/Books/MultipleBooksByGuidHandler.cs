using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;
using BookLovers.Publication.Infrastructure.Queries.Books;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.Books
{
    internal class MultipleBooksByGuidHandler : IQueryHandler<MultipleBooksByGuidQuery, List<BookDto>>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public MultipleBooksByGuidHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<List<BookDto>> HandleAsync(MultipleBooksByGuidQuery query)
        {
            var books = await _context.Books
                .Include(p => p.Authors)
                .Include(p => p.Publisher)
                .Include(p => p.Series)
                .Include(p => p.Reviews)
                .Include(p => p.Reader)
                .Include(p => p.Quotes)
                .AsNoTracking()
                .ActiveRecords()
                .WhereIf(p => query.Guides.Contains(p.Guid), query.Guides != null)
                .ToListAsync();

            return _mapper.Map<List<BookReadModel>, List<BookDto>>(books);
        }
    }
}