using BookLovers.Base.Domain.Services;
using BookLovers.Ratings.Domain;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Infrastructure.Root.Infrastructure;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Ratings.Infrastructure.Root.Domain
{
    internal class DomainModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Book))
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IAggregateManager<>)).BindAllInterfaces());

            Bind<RatingsService>().ToSelf();

            Bind<IBookInBookcaseChecker>().To<BookInBookcaseChecker>();
        }
    }
}