using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class ProfileReadModel : IReadModel<ProfileReadModel>
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public ReaderReadModel Reader { get; set; }

        public int ReaderId { get; set; }

        public string FullName { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public DateTime JoinedAt { get; set; }

        public DateTime? BirthDate { get; set; }

        public int Sex { get; set; }

        public string CurrentRole { get; set; }

        public string SexName { get; set; }

        public int Status { get; set; }

        public string About { get; set; }

        public string WebSite { get; set; }

        public IList<ProfileFavouriteReadModel> Favourites { get; set; }

        public ProfileReadModel()
        {
            Favourites = new List<ProfileFavouriteReadModel>();
        }
    }
}