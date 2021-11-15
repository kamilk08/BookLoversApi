using System.Collections.Generic;
using AutoMapper;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Seed.Deserializers;
using BookLovers.Seed.Mappings;
using BookLovers.Seed.Models;
using BookLovers.Seed.Services;
using BookLovers.Seed.Services.OpenLibrary;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Serilog;

namespace BookLovers.Seed.Root
{
    internal class ServicesModule : NinjectModule
    {
        private readonly ILogger _logger;

        public ServicesModule(ILogger logger)
        {
            _logger = logger;
        }

        public override void Load()
        {
            this.Bind<ILogger>().ToConstant(_logger);

            this.Bind<IAppManager>().To<AppManager>();

            this.Bind<IMapper>()
                .ToMethod(v => MappingConfiguration.CreateMapper());

            this.Bind<OpenLibraryBookDeserializer>().ToSelf();

            this.Bind(x =>
                x.FromThisAssembly()
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(ISeedFactory))
                    .BindAllInterfaces());

            this.Bind<BookRootsAccessor>()
                .ToSelf().InSingletonScope();

            this.Bind<SeedFactory>().ToSelf();

            this.Bind<Dictionary<SourceType, ISeedFactory>>()
                .ToProvider<SeedServiceProvider>();

            this.Bind<List<ISeedProvider>>()
                .ToProvider<SeedProviderFactory>();
        }
    }
}