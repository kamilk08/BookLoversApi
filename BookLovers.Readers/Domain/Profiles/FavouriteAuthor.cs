using System;

namespace BookLovers.Readers.Domain.Profiles
{
    public class FavouriteAuthor : BookLovers.Base.Domain.ValueObject.ValueObject<FavouriteAuthor>, IFavourite
    {
        public Guid AuthorGuid { get; }

        public Guid FavouriteGuid => AuthorGuid;

        public FavouriteType FavouriteType => FavouriteType.FavouriteAuthor;

        private FavouriteAuthor()
        {
        }

        public FavouriteAuthor(Guid authorGuid)
        {
            AuthorGuid = authorGuid;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.AuthorGuid.GetHashCode();
            hash = (hash * 23) + this.FavouriteType.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(FavouriteAuthor obj)
        {
            return AuthorGuid == obj.AuthorGuid
                   && FavouriteType == obj.FavouriteType;
        }
    }
}