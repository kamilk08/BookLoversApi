using System.Collections.Generic;
using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Contracts;
using BookLovers.Publication.Infrastructure.Mappings;
using BookLovers.Publication.Infrastructure.Root.Infrastructure.Providers;
using BookLovers.Publication.Infrastructure.Services;
using BookLovers.Publication.Infrastructure.Services.AuthorBooksSortingServices;
using BookLovers.Publication.Infrastructure.Services.PublisherBooksSortingServices;
using BookLovers.Publication.Infrastructure.Services.SeriesBooksSortingServices;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Ninject.Web.Common;

namespace BookLovers.Publication.Infrastructure.Root.Infrastructure
{
    internal class InfrastructureModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMapper>()
                .ToMethod(m => MappingConfiguration.ConfigureMapper())
                .InSingletonScope();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(IResourceService))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IResourceService)).BindAllInterfaces());

            Bind<IDictionary<IResourceService, ResourceType>>()
                .ToProvider<ResourcesProvider>();

            Bind<IResourceSaver>().To<ResourceSaver>();

            Bind<ReadContextAccessor>().ToSelf()
                .InRequestScope();

            Bind<IReadContextAccessor>().To<ReadContextAccessor>()
                .InRequestScope();

            Bind<IModule<PublicationModule>>().To<PublicationModule>();

            Bind<IAppManager>().To<AppManager>();

            this.Bind(x =>
                x.FromThisAssembly().IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IPublisherCollectionSorter))
                    .BindAllInterfaces());

            Bind<IDictionary<PublisherCollectionSortingType, IPublisherCollectionSorter>>()
                .ToProvider<PublisherCollectionSortingProvider>();

            this.Bind(x =>
                x.FromThisAssembly().IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(ISeriesCollectionSorter))
                    .BindAllInterfaces());

            Bind<IDictionary<SeriesCollectionSortingType, ISeriesCollectionSorter>>()
                .ToProvider<SeriesCollectionSortingProvider>();

            this.Bind(x =>
                x.FromThisAssembly().IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IAuthorCollectionSorter))
                    .BindAllInterfaces());

            Bind<IDictionary<AuthorCollectionSorType, IAuthorCollectionSorter>>()
                .ToProvider<AuthorsCollectionSortingProvider>();
        }
    }
}