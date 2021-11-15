using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Publication.Infrastructure.Services.SeriesBooksSortingServices
{
    public class SeriesCollectionSortingType : Enumeration
    {
        public static readonly SeriesCollectionSortingType ByTitle =
            new SeriesCollectionSortingType(1, "By Title");

        public static readonly SeriesCollectionSortingType ByAverage =
            new SeriesCollectionSortingType(2, "By Average");

        public static readonly SeriesCollectionSortingType ByPosition =
            new SeriesCollectionSortingType(3, "By Position");

        protected SeriesCollectionSortingType(int value, string name)
            : base(value, name)
        {
        }
    }
}