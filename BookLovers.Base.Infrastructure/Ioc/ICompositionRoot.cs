using Ninject;

namespace BookLovers.Base.Infrastructure.Ioc
{
    public interface ICompositionRoot
    {
        IKernel Kernel { get; }
    }
}