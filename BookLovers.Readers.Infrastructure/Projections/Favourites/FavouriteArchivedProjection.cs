using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Favourites;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.Projections.Favourites
{
    internal class FavouriteArchivedProjection :
        IProjectionHandler<FavouriteArchived>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public FavouriteArchivedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(FavouriteArchived @event)
        {
            this._context.Favourites.Where(p => p.FavouriteGuid == @event.AggregateGuid)
                .Update(p => new FavouriteReadModel
                {
                    Status = @event.Status
                });
        }
    }
}