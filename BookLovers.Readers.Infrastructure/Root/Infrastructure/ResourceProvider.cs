using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Readers.Application.Contracts;
using BookLovers.Readers.Infrastructure.Services;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Readers.Infrastructure.Root.Infrastructure
{
    public class ResourceProvider : BaseProvider<IDictionary<IResourceService, ResourceType>>
    {
        protected override IDictionary<IResourceService, ResourceType> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IResourceService>()
                .ToDictionary(k => k, v => v.ResourceType);
        }
    }
}