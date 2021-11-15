using System;

namespace BookLovers.Readers.Domain.Profiles
{
    public class FavouriteBook : BookLovers.Base.Domain.ValueObject.ValueObject<FavouriteBook>, IFavourite
    {
        public Guid BookGuid { get; }

        public Guid FavouriteGuid => BookGuid;

        public FavouriteType FavouriteType => FavouriteType.FavouriteBook;

        private FavouriteBook()
        {
        }

        public FavouriteBook(Guid bookGuid)
        {
            BookGuid = bookGuid;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.BookGuid.GetHashCode();
            hash = (hash * 23) + this.FavouriteType.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(FavouriteBook obj)
        {
            return BookGuid == obj.BookGuid && FavouriteType == obj.FavouriteType;
        }
    }
}