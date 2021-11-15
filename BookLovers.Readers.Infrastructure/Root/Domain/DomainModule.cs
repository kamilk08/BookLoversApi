using System.Collections.Generic;
using BookLovers.Base.Domain.Services;
using BookLovers.Readers.Domain.NotificationWalls.Services;
using BookLovers.Readers.Domain.Profiles;
using BookLovers.Readers.Domain.Profiles.Services.Factories;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Reviews;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Readers.Infrastructure.Root.Domain
{
    internal class DomainModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Reader))
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IFavouriteAdder))
                    .BindAllInterfaces());

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Reader))
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IFavouriteRemover))
                    .BindAllInterfaces());

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Reader))
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IResourceAdder))
                    .BindAllInterfaces());

            this.Bind<IDictionary<AddedResourceType, IResourceAdder>>()
                .ToProvider<ResourceAdderProvider>();

            this.Bind<IDictionary<FavouriteType, IFavouriteAdder>>()
                .ToProvider<FavouriteAdderProvider>();

            this.Bind<IDictionary<FavouriteType, IFavouriteRemover>>()
                .ToProvider<FavouriteRemoverProvider>();

            this.Bind<NotificationHider>().ToSelf();

            this.Bind<NotificationFactory>().ToSelf();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Reader))
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IAggregateManager<>))
                    .BindAllInterfaces());

            this.Bind<ProfileFactory>().ToSelf();

            this.Bind<ReviewFactory>().ToSelf();
        }
    }
}