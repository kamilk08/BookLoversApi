using System.Collections.Generic;
using BookLovers.Base.Domain.Services;
using BookLovers.Bookcases.Application.CommandHandlers.BookcaseBooks;
using BookLovers.Bookcases.Application.CommandHandlers.BookcaseOwners;
using BookLovers.Bookcases.Application.Contracts;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Domain.Services;
using BookLovers.Bookcases.Domain.Settings;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Bookcases.Infrastructure.Root.Domain
{
    internal class DomainModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Bookcase))
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IDomainService<>))
                    .BindAllInterfaces());

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Bookcase))
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(ISettingsChanger)).BindAllInterfaces());

            Bind<IDictionary<BookcaseOptionType, ISettingsChanger>>().ToProvider<SettingsChangerProvider>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Bookcase))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IAggregateManager<>))
                    .BindAllInterfaces());

            Bind<BookcaseFactory>().ToSelf();

            Bind<IBookcaseOwnerAccessor>().To<BookcaseOwnerAccessor>();

            Bind<IBookcaseBookAccessor>().To<BookcaseBookAccessor>();
        }
    }
}