using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Services;

namespace BookLovers.Readers.Domain.Profiles.Services
{
    public class FavouritesService : IDomainService<Profile>
    {
        private readonly IDictionary<FavouriteType, IFavouriteAdder> _adders;
        private readonly IDictionary<FavouriteType, IFavouriteRemover> _removers;

        public FavouritesService(
            IDictionary<FavouriteType, IFavouriteAdder> adders,
            IDictionary<FavouriteType, IFavouriteRemover> removers)
        {
            _adders = adders;
            _removers = removers;
        }

        public void AddFavourite(Profile profile, IFavourite favourite)
        {
            if (!_adders.ContainsKey(favourite.FavouriteType))
                throw new InvalidOperationException(
                    "Cannot add favourite object to profile. Favourite profile type is not valid");

            _adders[favourite.FavouriteType].AddFavourite(profile, favourite);
        }

        public void RemoveFavourite(Profile profile, IFavourite favourite)
        {
            if (!_removers.ContainsKey(favourite.FavouriteType))
                throw new InvalidOperationException(
                    "Cannot remove favourite object from profile. Favourite profile type is not valid");

            _removers[favourite.FavouriteType].RemoveFavourite(profile, favourite);
        }
    }
}