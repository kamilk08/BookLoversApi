using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;
using BookLovers.Ratings.Infrastructure.Queries.Books;

namespace BookLovers.Ratings.Infrastructure.QueryHandlers.Books
{
    internal class MultipleBooksHandler : IQueryHandler<MultipleBooksRatingsQuery, List<BookDto>>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public MultipleBooksHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<List<BookDto>> HandleAsync(
            MultipleBooksRatingsQuery ratingsQuery)
        {
            var multipleBooks = await this._context.Books
                .AsNoTracking()
                .Include(p => p.Ratings)
                .Where(p => ratingsQuery.BookIds.Contains(p.BookId))
                .ToListAsync();

            return this._mapper.Map<List<BookReadModel>, List<BookDto>>(multipleBooks);
        }
    }
}