using System;
using Ninject.Activation;

namespace BookLovers.Base.Infrastructure.Ioc
{
    public abstract class BaseProvider<T> : IProvider
    {
        public object Create(IContext context) => CreateInstance(context);

        protected abstract T CreateInstance(IContext context);

        public virtual Type Type { get; }
    }
}