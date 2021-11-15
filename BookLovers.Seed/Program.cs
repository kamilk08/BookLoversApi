using System;
using System.Threading.Tasks;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;
using BookLovers.Seed.Root;
using BookLovers.Seed.SeedExecutors;
using BookLovers.Seed.Services;
using Ninject;
using Serilog;
using Serilog.Exceptions;

namespace BookLovers.Seed
{
    internal class Program
    {
        private static readonly ILogger Logger = CreateLogger();
        private static readonly IKernel Kernel = ConfigureKernel();

        public static async Task Main(string[] args)
        {
            try
            {
                Logger.Information("Preparing seed...");

                var seedProviderService = Kernel.Get<SeedProviderService>();
                var seedExecutor = Kernel.Get<SeedExecutionService>();

                var openLibrarySetup = OpenLibrarySeedConfiguration.Initialize()
                    .WithDescription().WithLimit(80).WithIsbn().WithAuthors()
                    .WithSource(SourceType.OpenLibrary)
                    .PublicationRange(new DateTime(1960, 1, 1), DateTime.UtcNow);

                var ownResourceSetup = OwnResourceConfiguration.Initialize()
                    .WithUsers(25).WithSeries(25)
                    .WithTickets(15).WithQuotes(25)
                    .WithReviews(15);

                var config = new SeedDataConfig(openLibrarySetup, ownResourceSetup);

                var seed = await seedProviderService.CreateSeedDataAsync(config);

                await seedExecutor.SeedAsync(seed);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message.ToString(), "Error");
                Logger.Information("Seed was a failure.");

                return;
            }

            Logger.Information("Seed performed successfully.");

            Console.ReadLine();
        }

        private static IKernel ConfigureKernel()
        {
            return new StandardKernel(
                new ServicesModule(Program.Logger),
                new LoggingModule(),
                new SeedModule());
        }

        private static ILogger CreateLogger()
        {
            return Log.Logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger()
                .ForContext("Module", "SEED_MODULE", false);
        }
    }
}