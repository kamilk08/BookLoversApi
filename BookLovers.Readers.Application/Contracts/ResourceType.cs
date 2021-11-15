using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Readers.Application.Contracts
{
    public class ResourceType : Enumeration
    {
        public static readonly ResourceType Image = new ResourceType(1, nameof(Image));

        protected ResourceType(int value, string name)
            : base(value, name)
        {
        }
    }
}