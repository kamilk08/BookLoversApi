using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Profile
{
    public class FavouriteBookAdded : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public int FavouriteType { get; private set; }

        private FavouriteBookAdded()
        {
        }

        [JsonConstructor]
        protected FavouriteBookAdded(
            Guid guid,
            Guid aggregateGuid,
            Guid readerGuid,
            Guid bookGuid,
            int favouriteType)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            BookGuid = bookGuid;
            FavouriteType = favouriteType;
        }

        private FavouriteBookAdded(
            Guid aggregateGuid,
            Guid readerGuid,
            Guid bookGuid,
            int favouriteType)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            BookGuid = bookGuid;
            FavouriteType = favouriteType;
        }

        public static FavouriteBookAdded Initialize()
        {
            return new FavouriteBookAdded();
        }

        public FavouriteBookAdded WithAggregate(Guid aggregateGuid)
        {
            return new FavouriteBookAdded(aggregateGuid, ReaderGuid, BookGuid, FavouriteType);
        }

        public FavouriteBookAdded WithReader(Guid readerGuid)
        {
            return new FavouriteBookAdded(AggregateGuid, readerGuid, BookGuid, FavouriteType);
        }

        public FavouriteBookAdded WithBook(Guid bookGuid)
        {
            return new FavouriteBookAdded(AggregateGuid, ReaderGuid, bookGuid, FavouriteType);
        }

        public FavouriteBookAdded WithFavouriteType(int favouriteTypeId)
        {
            return new FavouriteBookAdded(AggregateGuid, ReaderGuid, BookGuid, favouriteTypeId);
        }
    }
}