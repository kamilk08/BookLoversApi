using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Shelf
{
    public class CoreShelfCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ShelfGuid { get; private set; }

        public string ShelfName { get; private set; }

        public int ShelfCategory { get; private set; }

        private CoreShelfCreated()
        {
        }

        private CoreShelfCreated(
            Guid bookcaseGuid,
            Guid shelfGuid,
            string shelfName,
            int shelfCategory)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = bookcaseGuid;
            ShelfGuid = shelfGuid;
            ShelfName = shelfName;
            ShelfCategory = shelfCategory;
        }

        public static CoreShelfCreated Initialize()
        {
            return new CoreShelfCreated();
        }

        public CoreShelfCreated WithBookcase(Guid bookcaseGuid)
        {
            return new CoreShelfCreated(bookcaseGuid, ShelfGuid, ShelfName, ShelfCategory);
        }

        public CoreShelfCreated WithShelf(string shelfName, int shelfCategory)
        {
            return new CoreShelfCreated(AggregateGuid, Guid.NewGuid(), shelfName, shelfCategory);
        }
    }
}