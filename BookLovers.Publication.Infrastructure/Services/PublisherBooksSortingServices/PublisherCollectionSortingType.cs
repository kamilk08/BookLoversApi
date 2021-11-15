using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Publication.Infrastructure.Services.PublisherBooksSortingServices
{
    public class PublisherCollectionSortingType : Enumeration
    {
        public static readonly PublisherCollectionSortingType ByTitle =
            new PublisherCollectionSortingType(1, "By title");

        public static readonly PublisherCollectionSortingType ByBookAverage =
            new PublisherCollectionSortingType(2, "By average");

        public static readonly PublisherCollectionSortingType ByPublicationDate =
            new PublisherCollectionSortingType(3, "By publication date");

        protected PublisherCollectionSortingType(int value, string name)
            : base(value, name)
        {
        }
    }
}