using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class ReaderReadModel : IReadModel<ReaderReadModel>
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int ReaderId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public DateTime JoinedAt { get; set; }

        public int Status { get; set; }

        public IList<FollowReadModel> Followers { get; set; }

        public IList<AddedResourceReadModel> AddedResources { get; set; }

        public int AddedResourcesCount { get; set; }

        public ReaderReadModel()
        {
            this.Followers = new List<FollowReadModel>();
            this.AddedResources = new List<AddedResourceReadModel>();
        }
    }
}