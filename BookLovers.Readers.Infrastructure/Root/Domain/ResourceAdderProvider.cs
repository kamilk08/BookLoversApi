using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Readers.Domain.Readers;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Readers.Infrastructure.Root.Domain
{
    public class ResourceAdderProvider : BaseProvider<IDictionary<AddedResourceType, IResourceAdder>>
    {
        public override Type Type => typeof(IDictionary<AddedResourceType, IResourceAdder>);

        protected override IDictionary<AddedResourceType, IResourceAdder> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IResourceAdder>()
                .ToDictionary(k => k.AddedResourceType, v => v);
        }
    }
}