using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Shelf
{
    public class ShelfRemoved : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ShelfGuid { get; private set; }

        private ShelfRemoved()
        {
        }

        public ShelfRemoved(Guid bookcaseGuid, Guid shelfGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = bookcaseGuid;
            ShelfGuid = shelfGuid;
        }
    }
}