namespace BookLovers.Readers.Domain.Profiles
{
    public interface IFavouriteRemover
    {
        FavouriteType FavouriteType { get; }

        void RemoveFavourite(Profile profile, IFavourite favourite);
    }
}