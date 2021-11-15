using System;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class ProfileFavouriteReadModel
    {
        public int Id { get; set; }

        public Guid FavouriteGuid { get; set; }

        public int FavouriteType { get; set; }

        public int ReaderId { get; set; }
    }
}