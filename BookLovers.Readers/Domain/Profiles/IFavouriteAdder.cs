namespace BookLovers.Readers.Domain.Profiles
{
    public interface IFavouriteAdder
    {
        FavouriteType FavouriteType { get; }

        void AddFavourite(Profile profile, IFavourite favourite);
    }
}