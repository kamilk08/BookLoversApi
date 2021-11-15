using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;
using BookLovers.Seed.Services;
using BookLovers.Seed.Services.OpenLibrary;
using BookLovers.Seed.Services.OpenLibrary.Loggers;
using BookLovers.Seed.Services.OwnResources;
using BookLovers.Seed.Services.OwnResources.Loggers;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Seed.Root
{
    internal class LoggingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISeedProvider<SeedAuthor>>()
                .To<OpenLibraryAuthorSeedProvider>()
                .WhenInjectedInto(typeof(OpenLibraryAuthorSeedLogger));

            Bind<IConfigurableSeedProvider<SeedBook, OpenLibrarySeedConfiguration>>()
                .To<OpenLibraryBooksSeedProvider>()
                .WhenInjectedInto(typeof(OpenLibraryBooksSeedLogger));

            Bind<ISeedProvider<SeedPublisher>>()
                .To<OpenLibraryPublisherSeedProvider>()
                .WhenInjectedInto(typeof(OpenLibraryPublisherSeedLogger));

            Bind<IConfigurableSeedProvider<SeedQuote, OwnResourceConfiguration>>()
                .To<OwnResourceSeedQuoteProvider>()
                .WhenInjectedInto(typeof(OwnResourceSeedQuoteLogger));

            Bind<IConfigurableSeedProvider<SeedReview, OwnResourceConfiguration>>()
                .To<OwnResourceSeedReviewProvider>()
                .WhenInjectedInto(typeof(OwnResourceSeedReviewLogger));

            Bind<IConfigurableSeedProvider<SeedSeries, OwnResourceConfiguration>>()
                .To<OwnResourceSeedSeriesProvider>()
                .WhenInjectedInto(typeof(OwnResourceSeedSeriesLogger));

            Bind<IConfigurableSeedProvider<SeedTicket, OwnResourceConfiguration>>()
                .To<OwnResourceSeedTicketsProvider>()
                .WhenInjectedInto(typeof(OwnResourceSeedTicketsLogger));

            Bind<IConfigurableSeedProvider<SeedUser, OwnResourceConfiguration>>()
                .To<OwnResourceSeedUserProvider>()
                .WhenInjectedInto(typeof(OwnResourceSeedUserLogger));

            this.Bind(x =>
                x.FromThisAssembly().IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .EndingWith("Logger")
                    .InheritedFrom(typeof(ISeedProvider))
                    .BindAllInterfaces());
        }
    }
}