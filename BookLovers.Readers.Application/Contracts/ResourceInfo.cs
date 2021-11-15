using BookLovers.Base.Infrastructure.Services;

namespace BookLovers.Readers.Application.Contracts
{
    public class ResourceInfo
    {
        public IResource Resource { get; }

        public ResourceType ResourceType { get; }

        public ResourceOwner ResourceOwner { get; }

        public ResourceInfo(IResource resource, ResourceType resourceType, ResourceOwner resourceOwner)
        {
            Resource = resource;
            ResourceType = resourceType;
            ResourceOwner = resourceOwner;
        }
    }
}