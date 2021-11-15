using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Favourites;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Projections.Favourites
{
    internal class FavouriteOwnerAddedProjection :
        IProjectionHandler<FavouriteOwnerAdded>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public FavouriteOwnerAddedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(FavouriteOwnerAdded @event)
        {
            var entity = new FavouriteOwnerReadModel()
            {
                OwnerGuid = @event.OwnerGuid
            };

            var favouriteReadModel = this._context.Favourites
                .Include(p => p.FavouriteOwners)
                .SingleOrDefault(p => p.FavouriteGuid == @event.AggregateGuid);

            favouriteReadModel.FavouriteOwners.Add(entity);

            this._context.FavouriteOwners.Add(entity);

            this._context.Favourites.AddOrUpdate(p => p.Id, favouriteReadModel);

            this._context.SaveChanges();
        }
    }
}