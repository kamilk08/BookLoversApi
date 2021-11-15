using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Favourites
{
    public class FavouriteOwner : ValueObject<FavouriteOwner>
    {
        public Guid OwnerGuid { get; private set; }

        private FavouriteOwner()
        {
        }

        public FavouriteOwner(Guid ownerGuid)
        {
            OwnerGuid = ownerGuid;
        }

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + OwnerGuid.GetHashCode();
        }

        protected override bool EqualsCore(FavouriteOwner obj)
        {
            return OwnerGuid == obj.OwnerGuid;
        }
    }
}