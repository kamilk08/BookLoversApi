using System;

namespace BookLovers.Readers.Application.WriteModels.Profiles
{
    public class ChangeFavouritesWriteModel
    {
        public Guid SocialProfileGuid { get; set; }

        public FavouritesWriteModel Favourites { get; set; }
    }
}