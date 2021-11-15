using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Seed.Services;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Seed.Root
{
    public class SeedProviderFactory : BaseProvider<List<ISeedProvider>>
    {
        protected override List<ISeedProvider> CreateInstance(IContext context)
        {
            return context.Kernel.GetAll<ISeedProvider>().ToList();
        }
    }
}