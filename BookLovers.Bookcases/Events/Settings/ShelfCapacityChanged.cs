using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Settings
{
    public class ShelfCapacityChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public int Capacity { get; private set; }

        private ShelfCapacityChanged()
        {
        }

        public ShelfCapacityChanged(Guid aggregateGuid, Guid bookcaseGuid, int capacity)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            BookcaseGuid = bookcaseGuid;
            Capacity = capacity;
        }
    }
}