using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Persistence;
using Ninject;

namespace BookLovers.Publication.Infrastructure.Root.Persistence
{
    public class MementoFactory : IMementoFactory
    {
        public IMemento<TAggregate> Create<TAggregate>()
            where TAggregate : class
        {
            return CompositionRoot.Kernel.Get<IMemento<TAggregate>>();
        }
    }
}