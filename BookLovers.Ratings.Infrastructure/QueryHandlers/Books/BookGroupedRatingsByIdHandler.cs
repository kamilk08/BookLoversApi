using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Domain.RatingStars;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Queries.Books;

namespace BookLovers.Ratings.Infrastructure.QueryHandlers.Books
{
    internal class BookGroupedRatingsByIdHandler :
        IQueryHandler<BookGroupedRatingsByIdQuery, BookGroupedRatingsDto>
    {
        private readonly RatingsContext _context;

        public BookGroupedRatingsByIdHandler(RatingsContext context)
        {
            this._context = context;
        }

        public async Task<BookGroupedRatingsDto> HandleAsync(
            BookGroupedRatingsByIdQuery query)
        {
            var ratings = await this._context.Books
                .Include(p => p.Ratings)
                .AsNoTracking()
                .Where(p => p.BookId == query.BookId)
                .SelectMany(sm => sm.Ratings)
                .Where(p => p.Stars > Star.Zero.Value)
                .GroupBy(p => p.Stars)
                .OrderByDescending(p => p.Key)
                .ToDictionaryAsync(k => k.Key, v => v.Count());

            StarList.Stars.Where(p => !ratings.ContainsKey(p.Value)).Skip(1)
                .ForEach(star => ratings.Add(star.Value, 0));

            return new BookGroupedRatingsDto()
            {
                BookId = query.BookId,
                GroupedRatings = ratings
            };
        }
    }
}