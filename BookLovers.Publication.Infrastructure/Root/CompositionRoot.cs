using BookLovers.Base.Infrastructure.Ioc;
using Ninject;

namespace BookLovers.Publication.Infrastructure.Root
{
    public class CompositionRoot : ICompositionRoot
    {
        private static IKernel _kernel;

        public static IKernel Kernel => _kernel;

        public static void SetKernel(IKernel kernel) => _kernel = kernel;

        IKernel ICompositionRoot.Kernel => _kernel;
    }
}