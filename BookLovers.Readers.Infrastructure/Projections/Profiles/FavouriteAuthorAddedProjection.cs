using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Profile;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Projections.Profiles
{
    internal class FavouriteAuthorAddedProjection :
        IProjectionHandler<FavouriteAuthorAdded>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public FavouriteAuthorAddedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(FavouriteAuthorAdded @event)
        {
            var profile = this._context.Profiles
                .Include(p => p.Favourites)
                .Include(p => p.Reader)
                .Single(p => p.Guid == @event.AggregateGuid);

            var readerReadModel = this._context.Readers
                .Single(p => p.ReaderId == profile.Reader.ReaderId);

            var favouriteReadModel = new ProfileFavouriteReadModel()
            {
                FavouriteGuid = @event.AuthorGuid,
                FavouriteType = @event.FavouriteType,
                ReaderId = readerReadModel.ReaderId
            };

            profile.Favourites.Add(favouriteReadModel);

            this._context.Profiles.AddOrUpdate(p => p.Id, profile);

            this._context.SaveChanges();
        }
    }
}