using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using AutoFixture;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Middleware;
using BookLovers.Middleware.OAuth;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ninject;
using NUnit.Framework;
using Owin;
using Serilog;
using SimpleE2ETesterLibrary.HttpClients;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace BaseTests.EndToEndHelpers
{
    [TestFixture]
    public abstract class EndToEndTest
    {
        protected Fixture Fixture;
        protected TestServer TestServer;
        protected HttpConfiguration HttpConfiguration;
        protected ILogger Logger;
        protected IKernel Kernel;
        protected ISimpleE2ETester E2ETester;

        [SetUp]
        protected async Task BeforeEachTest()
        {
            Fixture = new Fixture();
            ConfigureLogger();
            Log.Logger = Logger;

            HttpConfiguration = CreateHttpConfiguration();

            Kernel = CreateKernel();

            if (TestServer == null)
                TestServer = CreateTestServer();

            E2ETester = new SimpleE2ETester(new AspNetHttpClient(TestServer.HttpClient));

            await SendPreRequestsAsync();
        }

        protected abstract IKernel CreateKernel();

        protected abstract void RegisterServices(IKernel kernel);

        protected abstract void InitializeModules(
            IHttpContextAccessor contextAccessor,
            IAppManager manager);

        protected abstract Task SendPreRequestsAsync();

        protected abstract void ConfigureLogger();

        [TearDown]
        protected Task AfterEachTest()
        {
            TestServer?.Dispose();
            Kernel?.Dispose();
            Log.CloseAndFlush();

            return Task.CompletedTask;
        }

        private HttpConfiguration CreateHttpConfiguration()
        {
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();

            configuration.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new
            {
                id = RouteParameter.Optional
            });

            configuration.Services.Replace(typeof(IExceptionHandler), new FakeExceptionHandler(new FakeLogger()));
            configuration.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            configuration.Formatters.XmlFormatter.SupportedMediaTypes.Add(
                new MediaTypeHeaderValue("multipart/form-data"));

            configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            return configuration;
        }

        protected virtual TestServer CreateTestServer() => TestServer.Create(builder =>
        {
            builder.Use<InvalidAuthenticationMiddleware>();
            builder.UseOAuthServerAuthorizationMiddleware(Kernel);
            builder.UseOAuthBearerAuthenticationMiddleware(Kernel);
            builder.UseNinject(() => Kernel);
            builder.UseNinjectWebApi(HttpConfiguration);
        });
    }
}