using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Seed.Models;
using BookLovers.Seed.Services;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Seed.Root
{
    public class SeedServiceProvider : BaseProvider<Dictionary<SourceType, ISeedFactory>>
    {
        protected override Dictionary<SourceType, ISeedFactory> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<ISeedFactory>()
                .ToList().ToDictionary(k => k.SourceType, v => v);
        }
    }
}