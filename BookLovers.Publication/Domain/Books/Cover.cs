using BookLovers.Base.Domain.ValueObject;
using BookLovers.Publication.Domain.Books.CoverTypes;

namespace BookLovers.Publication.Domain.Books
{
    public class Cover : ValueObject<Cover>
    {
        public string CoverSource { get; }

        public CoverType CoverType { get; }

        private Cover()
        {
        }

        public Cover(CoverType coverType, string coverSource)
        {
            this.CoverType = coverType;
            this.CoverSource = coverSource;
        }

        public Cover(int coverTypeId, string coverSource)
        {
            this.CoverType = BookCovers.Get(coverTypeId);
            this.CoverSource = coverSource;
        }

        protected override bool EqualsCore(Cover obj)
        {
            return this.CoverSource == obj.CoverSource && this.CoverType == obj.CoverType;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.CoverSource.GetHashCode();
            hash = (hash * 23) + this.CoverType.GetHashCode();

            return hash;
        }
    }
}