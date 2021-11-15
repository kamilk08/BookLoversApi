using System.Collections.Generic;
using BookLovers.Base.Domain.Services;

namespace BookLovers.Readers.Domain.Readers.Services
{
    public class ResourceAdder : IDomainService<Reader>
    {
        private readonly IDictionary<AddedResourceType, IResourceAdder> _resourceAdders;

        public ResourceAdder(IDictionary<AddedResourceType, IResourceAdder> resourceAdders)
        {
            _resourceAdders = resourceAdders;
        }

        public void AddResource(Reader reader, IAddedResource addedResource)
        {
            _resourceAdders[addedResource.AddedResourceType]
                .AddResource(reader, addedResource);
        }
    }
}