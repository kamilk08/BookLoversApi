using BookLovers.Base.Infrastructure.Commands;
using Ninject.Modules;

namespace BookLovers.Bookcases.Infrastructure.Root.InternalProcessing
{
    internal class InternalProcessingModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IInternalCommandDispatcher>().To<InternalCommandDispatcher>();
        }
    }
}