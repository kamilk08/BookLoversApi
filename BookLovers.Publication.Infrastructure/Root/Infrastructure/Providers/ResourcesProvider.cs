using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Publication.Application.Contracts;
using BookLovers.Publication.Infrastructure.Services;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Publication.Infrastructure.Root.Infrastructure.Providers
{
    public class ResourcesProvider : BaseProvider<IDictionary<IResourceService, ResourceType>>
    {
        protected override IDictionary<IResourceService, ResourceType> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IResourceService>()
                .ToDictionary(k => k, v => v.ResourceType);
        }
    }
}