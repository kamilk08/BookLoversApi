using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Publication.Application.Contracts
{
    public class ResourceOwner : Enumeration
    {
        public static ResourceOwner Book = new ResourceOwner(1, nameof(Book));
        public static ResourceOwner Author = new ResourceOwner(2, nameof(Author));

        protected ResourceOwner(int value, string name)
            : base(value, name)
        {
        }
    }
}