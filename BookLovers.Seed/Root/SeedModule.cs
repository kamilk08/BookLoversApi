using System.Collections.Generic;
using BookLovers.Seed.SeedExecutors;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Seed.Root
{
    public class SeedModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x =>
                x.FromThisAssembly()
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(ISeedExecutor))
                    .BindAllInterfaces());

            Bind<IDictionary<SeedExecutorType, ISeedExecutor>>()
                .ToProvider<SeedExecutorsProvider>();

            Bind<SeedExecutionService>().ToSelf();
        }
    }
}