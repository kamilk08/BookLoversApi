using BookLovers.Base.Infrastructure.Commands;
using Ninject.Modules;

namespace BookLovers.Auth.Infrastructure.Root.InternalProcessing
{
    internal class InternalProcessingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IInternalCommandDispatcher>().To<InternalCommandDispatcher>();
        }
    }
}