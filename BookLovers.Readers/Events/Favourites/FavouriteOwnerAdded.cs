using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Readers.Events.Favourites
{
    public class FavouriteOwnerAdded : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid OwnerGuid { get; private set; }

        private FavouriteOwnerAdded()
        {
        }

        public FavouriteOwnerAdded(Guid aggregateGuid, Guid ownerGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            OwnerGuid = ownerGuid;
        }
    }
}