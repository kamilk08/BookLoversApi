using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Readers.Events.Favourites;

namespace BookLovers.Readers.Domain.Favourites
{
    public class Favourite :
        EventSourcedAggregateRoot,
        IHandle<FavouriteCreated>,
        IHandle<FavouriteArchived>,
        IHandle<FavouriteOwnerAdded>,
        IHandle<FavouriteOwnerRemoved>
    {
        private List<FavouriteOwner> _favouriteOwners = new List<FavouriteOwner>();

        public Guid FavouriteGuid { get; private set; }

        public int FavouriteId { get; private set; }

        public Guid AddedByGuid { get; private set; }

        public IReadOnlyList<FavouriteOwner> FavouriteOwners => _favouriteOwners;

        private Favourite()
        {
        }

        public Favourite(Guid favouriteGuid, int favouriteId, Guid addedByGuid)
        {
            Guid = favouriteGuid;
            FavouriteGuid = favouriteGuid;
            FavouriteId = favouriteId;
            AddedByGuid = addedByGuid;
            AggregateStatus = AggregateStatus.Active;

            ApplyChange(new FavouriteCreated(Guid, favouriteGuid, favouriteId));
        }

        public void AddFavouriteOwner(Guid ownerGuid)
        {
            ApplyChange(new FavouriteOwnerAdded(Guid, ownerGuid));
        }

        public void RemoveFavouriteOwner(Guid ownerGuid)
        {
            ApplyChange(new FavouriteOwnerRemoved(Guid, ownerGuid));
        }

        void IHandle<FavouriteCreated>.Handle(FavouriteCreated @event)
        {
            Guid = @event.AggregateGuid;
            FavouriteGuid = @event.FavouriteGuid;
            FavouriteId = @event.FavouriteId;
            AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<FavouriteOwnerAdded>.Handle(FavouriteOwnerAdded @event)
        {
            _favouriteOwners.Add(new FavouriteOwner(@event.OwnerGuid));
        }

        void IHandle<FavouriteOwnerRemoved>.Handle(
            FavouriteOwnerRemoved @event)
        {
            var favouriteOwner = _favouriteOwners.SingleOrDefault(
                p => p.OwnerGuid == @event.OwnerGuid);

            _favouriteOwners.Remove(favouriteOwner);
        }

        void IHandle<FavouriteArchived>.Handle(FavouriteArchived @event)
        {
            AggregateStatus = AggregateStatus.Archived;
        }
    }
}