using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Publication.Domain.Books.CoverTypes
{
    public class CoverType : Enumeration
    {
        public static CoverType PaperBack = new CoverType(1, "Paperback");
        public static CoverType HardCover = new CoverType(2, "Hardcover");
        public static CoverType NoCover = new CoverType(3, "No cover");

        protected CoverType()
        {
        }

        protected CoverType(int value, string name)
            : base(value, name)
        {
        }
    }
}