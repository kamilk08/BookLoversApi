using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Readers.Events.Favourites
{
    public class FavouriteArchived : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int Status { get; private set; }

        public List<Guid> FavouriteOwners { get; private set; }

        private FavouriteArchived()
        {
        }

        public FavouriteArchived(Guid aggregateGuid, List<Guid> favouriteOwners)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            Status = AggregateStatus.Archived.Value;
            FavouriteOwners = favouriteOwners;
        }
    }
}