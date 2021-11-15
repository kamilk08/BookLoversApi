using BookLovers.Base.Infrastructure;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence.Repositories;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Ratings.Infrastructure.Root.Persistence
{
    internal class PersistenceModule : NinjectModule
    {
        private readonly string _connectionString;

        public PersistenceModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override void Load()
        {
            Bind<RatingsContext>().ToSelf()
                .WithConstructorArgument("connectionString", _connectionString);

            Bind<IUnitOfWork>().To<UnitOfWork>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(AuthorRepository))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IRepository<>))
                    .BindAllInterfaces());
        }
    }
}