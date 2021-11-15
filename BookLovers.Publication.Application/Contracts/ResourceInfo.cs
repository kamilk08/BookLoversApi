using BookLovers.Base.Infrastructure.Services;

namespace BookLovers.Publication.Application.Contracts
{
    public class ResourceInfo
    {
        public IResource Resource { get; }

        public ResourceType ResourceType { get; }

        public ResourceOwner ResourceOwner { get; }

        public ResourceInfo(IResource resource, ResourceType resourceType, ResourceOwner resourceOwner)
        {
            this.Resource = resource;
            this.ResourceType = resourceType;
            this.ResourceOwner = resourceOwner;
        }
    }
}