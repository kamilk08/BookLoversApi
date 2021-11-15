using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Profile;
using BookLovers.Readers.Infrastructure.Persistence;

namespace BookLovers.Readers.Infrastructure.Projections.Profiles
{
    internal class FavouriteAuthorRemovedProjection :
        IProjectionHandler<FavouriteAuthorRemoved>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public FavouriteAuthorRemovedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(FavouriteAuthorRemoved @event)
        {
            var profileReadModel = this._context.Profiles
                .Include(p => p.Favourites)
                .Single(p => p.Guid == @event.AggregateGuid);

            var favouriteReadModel = profileReadModel.Favourites
                .Single(p => p.FavouriteGuid == @event.AuthorGuid);

            profileReadModel.Favourites.Remove(favouriteReadModel);

            this._context.Profiles.AddOrUpdate(p => p.Id, profileReadModel);

            this._context.SaveChanges();
        }
    }
}