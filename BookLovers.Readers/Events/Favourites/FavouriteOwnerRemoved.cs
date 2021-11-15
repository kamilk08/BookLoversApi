using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Readers.Events.Favourites
{
    public class FavouriteOwnerRemoved : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid OwnerGuid { get; private set; }

        private FavouriteOwnerRemoved()
        {
        }

        public FavouriteOwnerRemoved(Guid aggregateGuid, Guid ownerGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            OwnerGuid = ownerGuid;
        }
    }
}