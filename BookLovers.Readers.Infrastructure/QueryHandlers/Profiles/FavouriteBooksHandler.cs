using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.FilteringExtensions;
using BookLovers.Readers.Infrastructure.Queries.Readers.Profiles;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Profiles
{
    internal class FavouriteBooksHandler :
        IQueryHandler<FavouriteBooksQuery, IEnumerable<FavouriteBookDto>>
    {
        private readonly ReadersContext _context;

        public FavouriteBooksHandler(ReadersContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<FavouriteBookDto>> HandleAsync(
            FavouriteBooksQuery query)
        {
            var favouriteBooks = await _context
                .Profiles
                .AsNoTracking()
                .Include(p => p.Favourites)
                .SelectMany(sm => sm.Favourites)
                .WithReader(query.ReaderId)
                .OnlyFavouriteBooks()
                .Select(s => new FavouriteBookDto
                {
                    BookGuid = s.FavouriteGuid
                }).ToListAsync();

            return favouriteBooks;
        }
    }
}