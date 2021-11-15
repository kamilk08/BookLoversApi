using BookLovers.Base.Infrastructure;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Persistence.Repositories;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Librarians.Infrastructure.Root.Persistence
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
            Bind<LibrariansContext>().ToSelf()
                .WithConstructorArgument("connectionString", _connectionString);

            Bind<IUnitOfWork>().To<UnitOfWork>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(LibrarianRepository))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IRepository<>))
                    .BindAllInterfaces());
        }
    }
}