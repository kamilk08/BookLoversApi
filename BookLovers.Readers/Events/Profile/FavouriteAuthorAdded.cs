using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Profile
{
    public class FavouriteAuthorAdded : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public int FavouriteType { get; private set; }

        private FavouriteAuthorAdded()
        {
        }

        [JsonConstructor]
        protected FavouriteAuthorAdded(
            Guid guid,
            Guid aggregateGuid,
            Guid readerGuid,
            Guid authorGuid,
            int favouriteType)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            AuthorGuid = authorGuid;
            FavouriteType = favouriteType;
        }

        private FavouriteAuthorAdded(
            Guid aggregateGuid,
            Guid readerGuid,
            Guid authorGuid,
            int favouriteType)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            AuthorGuid = authorGuid;
            FavouriteType = favouriteType;
        }

        public static FavouriteAuthorAdded Initialize()
        {
            return new FavouriteAuthorAdded();
        }

        public FavouriteAuthorAdded WithAggregate(Guid aggregateGuid)
        {
            return new FavouriteAuthorAdded(aggregateGuid, ReaderGuid, AuthorGuid, FavouriteType);
        }

        public FavouriteAuthorAdded WithReader(Guid readerGuid)
        {
            return new FavouriteAuthorAdded(AggregateGuid, readerGuid, AuthorGuid, FavouriteType);
        }

        public FavouriteAuthorAdded WithAuthor(Guid authorGuid)
        {
            return new FavouriteAuthorAdded(AggregateGuid, ReaderGuid, authorGuid, FavouriteType);
        }

        public FavouriteAuthorAdded WithFavouriteType(int favouriteTypeId)
        {
            return new FavouriteAuthorAdded(AggregateGuid, ReaderGuid, AuthorGuid, favouriteTypeId);
        }
    }
}