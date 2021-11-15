using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Readers.Application.Contracts
{
    public class ResourceOwner : Enumeration
    {
        public static readonly ResourceOwner Avatar = new ResourceOwner(1, nameof(Avatar));

        protected ResourceOwner(int value, string name)
            : base(value, name)
        {
        }
    }
}