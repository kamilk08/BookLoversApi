using System;

namespace BookLovers.Readers.Domain.Profiles
{
    public interface IFavourite
    {
        Guid FavouriteGuid { get; }

        FavouriteType FavouriteType { get; }
    }
}