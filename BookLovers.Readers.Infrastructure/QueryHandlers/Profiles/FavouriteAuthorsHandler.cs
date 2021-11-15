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
    internal class FavouriteAuthorsHandler :
        IQueryHandler<FavouriteAuthorsQuery, IEnumerable<FavouriteAuthorDto>>
    {
        private readonly ReadersContext _context;

        public FavouriteAuthorsHandler(ReadersContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<FavouriteAuthorDto>> HandleAsync(
            FavouriteAuthorsQuery query)
        {
            var favouriteAuthors = await _context
                .Profiles
                .AsNoTracking()
                .Include(p => p.Favourites)
                .SelectMany(sm => sm.Favourites)
                .WithReader(query.ReaderId)
                .OnlyFavouriteAuthors()
                .Select(s => new FavouriteAuthorDto
                {
                    AuthorGuid = s.FavouriteGuid
                }).ToListAsync();

            return favouriteAuthors;
        }
    }
}