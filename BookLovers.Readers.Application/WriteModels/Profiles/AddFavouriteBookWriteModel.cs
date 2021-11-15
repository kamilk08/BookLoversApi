using System;

namespace BookLovers.Readers.Application.WriteModels.Profiles
{
    public class AddFavouriteBookWriteModel
    {
        public Guid ProfileGuid { get; set; }

        public Guid BookGuid { get; set; }

        public DateTime AddedAt { get; set; }
    }
}