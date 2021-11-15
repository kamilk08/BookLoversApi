using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Seed.SeedExecutors
{
    public class SeedExecutorsProvider : BaseProvider<IDictionary<SeedExecutorType, ISeedExecutor>>
    {
        public override Type Type => typeof(IDictionary<SeedExecutorType, ISeedExecutor>);

        protected override IDictionary<SeedExecutorType, ISeedExecutor> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<ISeedExecutor>()
                .ToDictionary(k => k.ExecutorType, v => v);
        }
    }
}