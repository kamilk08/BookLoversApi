using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class ProfilePrivacyManagerReadModel :
        IReadModel<ProfilePrivacyManagerReadModel>
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid ProfileGuid { get; set; }

        public int ProfileId { get; set; }

        public int ReaderId { get; set; }

        public int Status { get; set; }

        public IList<ProfilePrivacyOptionReadModel> PrivacyOptions { get; set; }

        public ProfilePrivacyManagerReadModel()
        {
            this.PrivacyOptions = new List<ProfilePrivacyOptionReadModel>();
        }
    }
}