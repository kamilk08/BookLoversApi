using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Favourites;
using BookLovers.Readers.Infrastructure.Persistence;

namespace BookLovers.Readers.Infrastructure.Projections.Favourites
{
    internal class FavouriteOwnerRemovedProjection :
        IProjectionHandler<FavouriteOwnerRemoved>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public FavouriteOwnerRemovedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(FavouriteOwnerRemoved @event)
        {
            var favouriteReadModel = this._context.Favourites
                .Include(p => p.FavouriteOwners)
                .SingleOrDefault(p => p.FavouriteGuid == @event.AggregateGuid);

            var favouriteOwnerReadModel = favouriteReadModel.FavouriteOwners
                .SingleOrDefault(p => p.OwnerGuid == @event.OwnerGuid);

            favouriteReadModel.FavouriteOwners.Remove(favouriteOwnerReadModel);

            this._context.Favourites.AddOrUpdate(p => p.Id, favouriteReadModel);

            this._context.SaveChanges();
        }
    }
}