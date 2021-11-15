using AutoMapper;
using BookLovers.Auth.Application.Contracts.Tokens;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Infrastructure.Mappings;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Auth.Infrastructure.Services.Authentication;
using BookLovers.Auth.Infrastructure.Services.Authorization;
using BookLovers.Auth.Infrastructure.Services.Hashing;
using BookLovers.Auth.Infrastructure.Services.Tokens;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Ninject.Web.Common;

namespace BookLovers.Auth.Infrastructure.Root.Infrastructure
{
    internal class ServicesModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IModule<AuthModule>>().To<AuthModule>();

            this.Bind(x => x.FromThisAssembly()
                .SelectAllClasses()
                .InheritedFrom(typeof(IHasher))
                .BindAllInterfaces());

            this.Bind<IAuthorizeService>().To<AuthorizationService>();

            this.Bind<IHashingService>().To<HashingService>();

            this.Bind<IMapper>().ToMethod(ctx => AuthMapperConfiguration.CreateMapper()).InSingletonScope();

            this.Bind<IReadContextAccessor>().To<ReadContextAccessor>().InRequestScope();

            this.Bind<IRandomSecretKeyGenerator>().To<RandomSecretKeyGenerator>();

            this.Bind(x => x.FromThisAssembly()
                .IncludingNonPublicTypes()
                .SelectAllClasses()
                .InheritedFrom(typeof(ITokenWriter<>))
                .BindAllInterfaces());

            this.Bind(x =>
                x.FromThisAssembly()
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(ITokenDescriptor<>)).BindAllInterfaces());

            this.Bind<ITokenManager>().To<TokenManager>();

            this.Bind<JwtTokenReader>().ToSelf();

            this.Bind<ClaimsBuilder>().ToSelf();
        }
    }
}