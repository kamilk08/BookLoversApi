using System;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class FavouriteOwnerReadModel
    {
        public int Id { get; set; }

        public Guid OwnerGuid { get; set; }
    }
}