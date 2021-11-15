using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Shared.Likes
{
    public class Like : ValueObject<Like>
    {
        public Guid ReaderGuid { get; }

        protected Like()
        {
        }

        protected Like(Guid readerGuid)
        {
            ReaderGuid = readerGuid;
        }

        public static Like NewLike(Guid readerGuid)
        {
            return new Like(readerGuid);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = hash * 23 + this.ReaderGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(Like obj)
        {
            return this.ReaderGuid == obj.ReaderGuid;
        }
    }
}