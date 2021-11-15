using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Infrastructure.Mappings;
using Ninject.Modules;

namespace BookLovers.Librarians.Infrastructure.Root.Infrastructure
{
    internal class InfrastructureModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMapper>()
                .ToMethod(m => Configuration.Configure()).InSingletonScope();

            Bind<IModule<LibrarianModule>>().To<LibrarianModule>();

            Bind<IValidationDecorator<LibrarianModule>>().To<ModuleValidationDecorator>();

            Bind<IReadContextAccessor>().To<ReadContextAccessor>();

            Bind<ReadContextAccessor>().ToSelf();
        }
    }
}