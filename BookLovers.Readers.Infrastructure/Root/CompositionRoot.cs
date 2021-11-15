using BookLovers.Base.Infrastructure.Ioc;
using Ninject;

namespace BookLovers.Readers.Infrastructure.Root
{
    public class CompositionRoot : ICompositionRoot
    {
        private static IKernel _kernel;

        public static IKernel Kernel => CompositionRoot._kernel;

        public static void SetKernel(IKernel kernel) => CompositionRoot._kernel = kernel;

        IKernel ICompositionRoot.Kernel => CompositionRoot._kernel;
    }
}