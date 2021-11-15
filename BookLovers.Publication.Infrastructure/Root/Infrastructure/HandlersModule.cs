using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Application.Commands.Books;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Publication.Infrastructure.Root.Infrastructure
{
    internal class HandlersModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x =>
                x.FromAssemblyContaining(typeof(AddBookCommand))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(ICommandHandler<>))
                    .BindAllInterfaces());

            this.Bind(x =>
                x.FromThisAssembly().IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IQueryHandler<,>))
                    .BindAllInterfaces());
        }
    }
}