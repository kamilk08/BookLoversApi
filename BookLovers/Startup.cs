using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Infrastructure.Queries.Users;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Bookcases.Infrastructure.Root;
using BookLovers.Librarians.Infrastructure.Root;
using BookLovers.Middleware;
using BookLovers.Middleware.OAuth;
using BookLovers.Modules;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Queries.Authors;
using BookLovers.Publication.Infrastructure.Queries.Publishers;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Ratings.Infrastructure.Root;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Services;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;
using Ninject;
using Nito.AsyncEx;
using Owin;
using Serilog;
using Serilog.Exceptions;

namespace BookLovers
{
    public class Startup
    {
        private static ILogger _logger;

        public void Configuration(IAppBuilder appBuilder)
        {
            var kernel = CreateKernel();

            var httpConfiguration = CreateHttpConfiguration();

            appBuilder
                .Use<InvalidAuthenticationMiddleware>()
                .UseCors(CorsOptions.AllowAll)
                .UseOAuthServerAuthorizationMiddleware(kernel)
                .UseOAuthBearerAuthenticationMiddleware(kernel)
                .UseNinject(() => kernel).UseNinjectWebApi(httpConfiguration);
        }

        private HttpConfiguration CreateHttpConfiguration()
        {
            var configuration = new HttpConfiguration();

            configuration.MapHttpAttributeRoutes();

            configuration.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new
            {
                id = RouteParameter.Optional
            });

            configuration.Routes.MapHttpRoute("NotFound", "{*uri}", new
            {
                controller = "NotFound",
                uri = RouteParameter.Optional
            });

            configuration.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler(_logger));
            configuration.Formatters.Clear();

            configuration.Formatters.Add(new JsonMediaTypeFormatter());

            configuration.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(
                new MediaTypeHeaderValue("multipart/form-data"));

            configuration.MessageHandlers.Add(new MethodNotAllowedHandler());
            configuration.MessageHandlers.Add(new NotFoundHandler());

            return configuration;
        }

        private IKernel CreateKernel()
        {
            var root = new StandardKernel(
                new CacheModule(),
                new ApiModule());

            RegisterServices(root);

            ConfigureLogger();

            var httpContextAccessor = root.Get<IHttpContextAccessor>();
            var appManager = root.Get<IAppManager>();

            AuthModuleStartup.Initialize(httpContextAccessor, appManager, _logger,
                PersistenceSettings.DropAndSeedAgain());

            ReadersModuleStartup.Initialize(httpContextAccessor, appManager, _logger,
                PersistenceSettings.DropAndSeedAgain());

            BookcaseModuleStartup.Initialize(httpContextAccessor, appManager, _logger,
                PersistenceSettings.DropAndSeedAgain());

            LibrarianModuleStartup.Initialize(httpContextAccessor, appManager, _logger,
                PersistenceSettings.DropAndSeedAgain());

            PublicationModuleStartup.Initialize(httpContextAccessor, appManager, _logger,
                PersistenceSettings.DropAndSeedAgain());

            RatingsModuleStartup.Initialize(httpContextAccessor, appManager, _logger,
                PersistenceSettings.DropAndSeedAgain());

            var authModule = root.Get<IModule<AuthModule>>();
            var booksModule = root.Get<IModule<PublicationModule>>();
            var userGuid = Guid.NewGuid();

            AsyncContext.Run(async () => await CreateSuperAdminAsync(authModule, appManager, userGuid));
            AsyncContext.Run(async () => await CreateSelfPublishedAsync(booksModule, appManager));
            AsyncContext.Run(async () => await CreateUnknownAuthorAsync(booksModule, appManager, userGuid));

            return root;
        }

        private void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IModule<AuthModule>>().To<AuthModule>();
            kernel.Bind<IModule<LibrarianModule>>().To<LibrarianModule>();
            kernel.Bind<IModule<ReadersModule>>().To<ReadersModule>();
            kernel.Bind<IModule<PublicationModule>>().To<PublicationModule>();
            kernel.Bind<IModule<BookcaseModule>>().To<BookcaseModule>();
            kernel.Bind<IModule<RatingsModule>>().To<RatingsModule>();
            kernel.Bind<IHttpContextAccessor>().To<HttpContextAccessor>();
            kernel.Bind<IAppManager>().To<AppManager>();
            kernel.Bind<ITokenManager>().To<TokenManager>();
            kernel.Bind<ILogger>().ToConstant(_logger).InSingletonScope();
        }

        private async Task CreateSuperAdminAsync(
            IModule<AuthModule> authModule,
            IAppManager appManager,
            Guid userGuid)
        {
            var queryResult =
                await authModule.ExecuteQueryAsync<IsSuperAdminCreatedQuery, bool>(new IsSuperAdminCreatedQuery());

            var command = CreateSuperAdminCommand.Create(userGuid);

            if (queryResult.Value)
                return;

            var validationResult = await authModule.SendCommandAsync(command);
        }

        private async Task CreateUnknownAuthorAsync(
            IModule<PublicationModule> booksModule,
            IAppManager appManager,
            Guid userGuid)
        {
            var queryResult = await booksModule.ExecuteQueryAsync<AuthorByNameQuery, AuthorDto>(
                new AuthorByNameQuery(UnknownAuthor.Key));

            if (queryResult.Value != null)
                return;

            var validationResult =
                await booksModule.SendCommandAsync(new CreateUnknownAuthorCommand(Guid.NewGuid(), userGuid));
        }

        private async Task CreateSelfPublishedAsync(
            IModule<PublicationModule> booksModule,
            IAppManager appManager)
        {
            var queryResult = await booksModule.ExecuteQueryAsync<PublisherByNameQuery, PublisherDto>(
                new PublisherByNameQuery(SelfPublisher.Key));

            if (queryResult.Value != null)
                return;

            var validationResult = await booksModule.SendCommandAsync(new CreateSelfPublisherCommand(Guid.NewGuid()));
        }

        private void ConfigureLogger()
        {
            _logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .WriteTo.File(HostingEnvironment.ApplicationPhysicalPath + "/logs.txt")
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            _logger = _logger.ForContext("Module", "API_MODULE");
            Log.Logger = _logger;
        }
    }
}