using BookLovers.Auth.Domain.Roles;
using BookLovers.Auth.Domain.Tokens.Services;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Domain.Users.Services.Factories;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Auth.Infrastructure.Root.Domain
{
    internal class DomainModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x
                .FromAssemblyContaining(typeof(User))
                .SelectAllClasses()
                .InheritedFrom(typeof(IDomainService<>))
                .BindAllInterfaces());

            this.Bind(x =>
                x.FromThisAssembly()
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IRepository<>))
                    .BindAllInterfaces());

            Bind<IEmailUniquenessChecker>().To<UserUniquenessChecker>();

            Bind<IUserNameUniquenessChecker>().To<UserUniquenessChecker>();

            Bind<RoleRulesCollection>().ToSelf();

            Bind<UserFactorySetup>().ToSelf();

            Bind<SuperAdminFactorySetup>().ToSelf();

            Bind<RefreshTokenFactory>().ToSelf();

            Bind<UserAuthenticationService>().ToSelf();
        }
    }
}