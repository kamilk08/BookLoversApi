using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Publication.Infrastructure.Services.AuthorBooksSortingServices
{
    public class AuthorCollectionSorType : Enumeration
    {
        public static readonly AuthorCollectionSorType ByTitle = new AuthorCollectionSorType(1, nameof(ByTitle));
        public static readonly AuthorCollectionSorType ByAverage = new AuthorCollectionSorType(2, "By Average");

        protected AuthorCollectionSorType(int value, string name)
            : base(value, name)
        {
        }
    }
}