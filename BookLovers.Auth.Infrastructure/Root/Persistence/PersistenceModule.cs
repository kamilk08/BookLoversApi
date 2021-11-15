using BookLovers.Auth.Domain.Roles;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure;
using Ninject.Modules;

namespace BookLovers.Auth.Infrastructure.Root.Persistence
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
            Bind<AuthContext>().ToSelf()
                .WithConstructorArgument("connectionString", _connectionString);

            Bind<IUnitOfWork>().To<UnitOfWork>();

            Bind<IRoleProvider>().To<RoleProvider>();
        }
    }
}