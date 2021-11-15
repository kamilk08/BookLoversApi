using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Readers.Events.Favourites
{
    public class FavouriteCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid FavouriteGuid { get; private set; }

        public int FavouriteId { get; private set; }

        private FavouriteCreated()
        {
        }

        public FavouriteCreated(Guid aggregateGuid, Guid favouriteGuid, int favouriteId)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            FavouriteGuid = favouriteGuid;
            FavouriteId = favouriteId;
        }
    }
}