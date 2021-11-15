using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class FavouriteReadModel : IReadModel<FavouriteReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid FavouriteGuid { get; set; }

        public List<FavouriteOwnerReadModel> FavouriteOwners { get; set; }

        public int Status { get; set; }

        public FavouriteReadModel()
        {
            FavouriteOwners = new List<FavouriteOwnerReadModel>();
        }
    }
}