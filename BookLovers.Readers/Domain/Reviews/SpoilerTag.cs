using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Reviews
{
    public class SpoilerTag : ValueObject<SpoilerTag>
    {
        public Guid ReaderGuid { get; }

        private SpoilerTag()
        {
        }

        public SpoilerTag(Guid readerGuid)
        {
            ReaderGuid = readerGuid;
        }

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + ReaderGuid.GetHashCode();
        }

        protected override bool EqualsCore(SpoilerTag obj)
        {
            return ReaderGuid == obj.ReaderGuid;
        }
    }
}