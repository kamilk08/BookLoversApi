using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Bookcases.Infrastructure.Services
{
    public class BookcaseCollectionSortType : Enumeration
    {
        public static readonly BookcaseCollectionSortType ByDate =
            new BookcaseCollectionSortType(1, "By Date");

        public static readonly BookcaseCollectionSortType ByBookAverage =
            new BookcaseCollectionSortType(2, "By Book Average");

        protected BookcaseCollectionSortType(int value, string name)
            : base(value, name)
        {
        }
    }
}