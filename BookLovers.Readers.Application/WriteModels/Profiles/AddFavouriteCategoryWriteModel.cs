using System;

namespace BookLovers.Readers.Application.WriteModels.Profiles
{
    public class AddFavouriteCategoryWriteModel
    {
        public Guid ProfileGuid { get; set; }

        public byte CategoryId { get; set; }
    }
}