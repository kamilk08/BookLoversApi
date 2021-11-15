using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Shelf
{
    public class ShelfNameChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ShelfGuid { get; private set; }

        public string ShelfName { get; private set; }

        private ShelfNameChanged()
        {
        }

        public ShelfNameChanged(Guid aggregateGuid, Guid shelfGuid, string shelfName)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ShelfGuid = shelfGuid;
            ShelfName = shelfName;
        }
    }
}