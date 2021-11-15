using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.Favourites;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Projections.Favourites
{
    internal class FavouriteCreatedProjection :
        IProjectionHandler<FavouriteCreated>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public FavouriteCreatedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(FavouriteCreated @event)
        {
            this._context.Favourites.Add(new FavouriteReadModel()
            {
                FavouriteGuid = @event.FavouriteGuid,
                Status = AggregateStatus.Active.Value
            });

            this._context.SaveChanges();
        }
    }
}