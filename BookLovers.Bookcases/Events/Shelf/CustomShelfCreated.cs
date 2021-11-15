using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Shelf
{
    public class CustomShelfCreated : IEvent
    {
        public Guid ShelfGuid { get; private set; }

        public string ShelfName { get; private set; }

        public int ShelfCategory { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        private CustomShelfCreated()
        {
        }

        public CustomShelfCreated(
            Guid bookcaseGuid,
            Guid shelfGuid,
            string shelfName,
            int shelfCategory)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = bookcaseGuid;
            ShelfCategory = shelfCategory;
            ShelfName = shelfName;
            ShelfGuid = shelfGuid;
        }

        public CustomShelfCreated WithAggregate(Guid bookcaseGuid)
        {
            return new CustomShelfCreated(bookcaseGuid, ShelfGuid, ShelfName, ShelfCategory);
        }

        public CustomShelfCreated WithShelf(Guid shelfGuid, string shelfName, int shelfCategory)
        {
            return new CustomShelfCreated(AggregateGuid, shelfGuid, shelfName, shelfCategory);
        }
    }
}