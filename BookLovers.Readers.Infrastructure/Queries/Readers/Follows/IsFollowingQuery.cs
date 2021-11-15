using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.Follows
{
    public class IsFollowingQuery : IQuery<bool>
    {
        public int FollowingId { get; set; }

        public IsFollowingQuery()
        {
        }

        public IsFollowingQuery(int followingId)
        {
            this.FollowingId = followingId;
        }
    }
}