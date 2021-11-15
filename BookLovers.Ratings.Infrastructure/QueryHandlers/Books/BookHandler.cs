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
    internal class BookHandler : IQueryHandler<BookByIdQuery, BookDto>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public BookHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<BookDto> HandleAsync(BookByIdQuery byIdQuery)
        {
            var book = await this._context.Books.AsNoTracking().Include(p => p.Ratings)
                .SingleOrDefaultAsync(p => p.BookId == byIdQuery.BookId);

            return this._mapper.Map<BookReadModel, BookDto>(book);
        }
    }
}