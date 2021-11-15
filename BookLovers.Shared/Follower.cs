using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Shared
{
    public sealed class Follower : ValueObject<Follower>
    {
        public Guid FollowedBy { get; private set; }

        private Follower()
        {
        }

        public Follower(Guid followedBy)
        {
            FollowedBy = followedBy;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;
            hash = hash * 23 + FollowedBy.GetHashCode();
            return hash;
        }

        protected override bool EqualsCore(Follower obj)
        {
            return this.FollowedBy == obj.FollowedBy;
        }
    }
}