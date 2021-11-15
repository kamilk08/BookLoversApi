using System;

namespace BookLovers.Readers.Application.WriteModels.Profiles
{
    public class RemoveFavouriteWriteModel
    {
        public Guid ProfileGuid { get; set; }

        public Guid FavouriteGuid { get; set; }
    }
}