using System.Collections.Generic;
using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Contracts;
using BookLovers.Readers.Infrastructure.Mappings;
using BookLovers.Readers.Infrastructure.Services;
using BookLovers.Readers.Infrastructure.Services.BookReviewsSortingServices;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Readers.Infrastructure.Root.Infrastructure
{
    internal class InfrastructureModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IMapper>()
                .ToMethod(b => MappingConfiguration.ConfigureMapper()).InSingletonScope();

            this.Bind<IModule<ReadersModule>>().To<ReadersModule>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(IResourceService))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IResourceService))
                    .BindAllInterfaces());

            this.Bind<IDictionary<IResourceService, ResourceType>>()
                .ToProvider<ResourceProvider>();

            this.Bind<IResourceSaver>().To<ResourceSaver>();

            this.Bind<IReadContextAccessor>().To<ReadContextAccessor>();

            this.Bind(x =>
                x.FromThisAssembly().IncludingNonPublicTypes()
                    .SelectAllClasses().InheritedFrom(typeof(IReviewsSorter))
                    .BindAllInterfaces());

            this.Bind<IDictionary<ReviewsSortingType, IReviewsSorter>>()
                .ToProvider<ReviewsSortersProvider>();
        }
    }
}