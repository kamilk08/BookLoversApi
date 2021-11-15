using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Application.Commands;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Librarians.Infrastructure.Root.Infrastructure
{
    internal class HandlersModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x
                .FromAssemblyContaining(typeof(CreateLibrarianCommand))
                .IncludingNonPublicTypes().SelectAllClasses()
                .InheritedFrom(typeof(ICommandHandler<>))
                .BindAllInterfaces());

            this.Bind(x => x
                .FromThisAssembly()
                .IncludingNonPublicTypes()
                .SelectAllClasses()
                .InheritedFrom(typeof(IQueryHandler<,>))
                .BindAllInterfaces());
        }
    }
}