using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Application.Commands.Shelves;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Bookcases.Infrastructure.Root.Infrastructure
{
    internal class HandlersModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x =>
                x.FromThisAssembly()
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IQueryHandler<,>))
                    .BindAllInterfaces());

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(AddShelfCommand))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(ICommandHandler<>))
                    .BindAllInterfaces());
        }
    }
}