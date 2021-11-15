using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Queries;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Auth.Infrastructure.Root.Infrastructure
{
    internal class HandlersModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x.FromAssemblyContaining(typeof(BlockAccountCommand))
                .IncludingNonPublicTypes().SelectAllClasses()
                .InheritedFrom(typeof(ICommandHandler<>))
                .BindAllInterfaces());

            this.Bind(x => x.FromThisAssembly()
                .IncludingNonPublicTypes()
                .SelectAllClasses()
                .InheritedFrom(typeof(IQueryHandler<,>))
                .BindAllInterfaces());
        }
    }
}