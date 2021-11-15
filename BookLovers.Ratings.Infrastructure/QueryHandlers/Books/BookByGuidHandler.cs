using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;
using BookLovers.Ratings.Infrastructure.Queries.Books;

namespace BookLovers.Ratings.Infrastructure.QueryHandlers.Books
{
    internal class BookByGuidHandler : IQueryHandler<BookByGuidQuery, BookDto>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public BookByGuidHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<BookDto> HandleAsync(BookByGuidQuery query)
        {
            var book = await this._context.Books.AsNoTracking()
                .Include(p => p.Ratings)
                .Include(p => p.Authors)
                .SingleOrDefaultAsync(p => p.BookGuid == query.BookGuid);

            return this._mapper.Map<BookReadModel, BookDto>(book);
        }
    }
}