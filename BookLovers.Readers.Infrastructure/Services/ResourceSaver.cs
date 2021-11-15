using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Readers.Application.Contracts;

namespace BookLovers.Readers.Infrastructure.Services
{
    public class ResourceSaver : IResourceSaver
    {
        private readonly IDictionary<IResourceService, ResourceType> _resourceServices;

        public ResourceSaver(
            IDictionary<IResourceService, ResourceType> resourceServices)
        {
            this._resourceServices = resourceServices;
        }

        public async Task SaveResourceAsync(Guid resourceGuid, ResourceInfo resourceInfo)
        {
            await this.GetResourceService(resourceInfo).SaveResourceAsync(resourceInfo.Resource, resourceGuid);
        }

        private IResourceService GetResourceService(ResourceInfo resourceInfo)
        {
            return this._resourceServices
                .SingleOrDefault(p => p.Value == resourceInfo.ResourceType
                                      && p.Key.ResourceOwner ==
                                      resourceInfo.ResourceOwner).Key;
        }
    }
}