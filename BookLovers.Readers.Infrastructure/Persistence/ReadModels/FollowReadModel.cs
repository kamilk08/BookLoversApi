using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class FollowReadModel : IReadModel
    {
        public int Id { get; set; }

        public ReaderReadModel Followed { get; set; }

        public ReaderReadModel Follower { get; set; }
    }
}