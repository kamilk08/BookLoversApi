using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Photos.Models.Authors;
using BookLovers.Photos.Models.Avatars;
using BookLovers.Photos.Models.Books;
using BookLovers.Photos.Models.Services;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Photos.Models
{
    public class PhotosModule : NinjectModule
    {
        private readonly IAppManager _manager;

        public PhotosModule(IAppManager manager)
        {
            this._manager = manager;
        }

        public override void Load()
        {
            var readersConnectionString = this._manager.GetConfigValue(ConnectionStrings.ReadersConnectionStringKey);
            var publicationsConnectionString =
                this._manager.GetConfigValue(ConnectionStrings.PublicationsConnectionStringKey);

            this.Bind<SqlClient>()
                .ToSelf()
                .WhenInjectedInto<AvatarPathFactory>()
                .WithConstructorArgument("connectionString", readersConnectionString);

            this.Bind<SqlClient>()
                .ToSelf()
                .WhenInjectedInto<AuthorImagePathFactory>()
                .WithConstructorArgument("connectionString", publicationsConnectionString);

            this.Bind<SqlClient>()
                .ToSelf()
                .WhenInjectedInto<CoverPathFactory>()
                .WithConstructorArgument("connectionString", publicationsConnectionString);

            this.Bind(x =>
                x.FromThisAssembly().IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IFilePathFactory))
                    .BindAllInterfaces());

            this.Bind<IDictionary<ProviderType, IFilePathFactory>>()
                .ToProvider<FilePathProvider>();
        }
    }
}