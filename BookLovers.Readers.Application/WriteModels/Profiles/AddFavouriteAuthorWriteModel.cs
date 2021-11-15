using System;

namespace BookLovers.Readers.Application.WriteModels.Profiles
{
    public class AddFavouriteAuthorWriteModel
    {
        public Guid AuthorGuid { get; set; }

        public Guid ProfileGuid { get; set; }

        public DateTime AddedAt { get; set; }
    }
}