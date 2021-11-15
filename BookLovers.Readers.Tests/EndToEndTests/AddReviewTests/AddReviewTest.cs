using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.HttpRequests.Auth;
using BaseTests.EndToEndHelpers.HttpRequests.Librarians;
using BaseTests.EndToEndHelpers.HttpRequests.Publications;
using BaseTests.EndToEndHelpers.HttpRequests.Reviews;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Root;
using BookLovers.Modules;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Readers.Store.Persistence;
using BookLovers.Readers.Tests.EndToEndTests.AddFavouriteAuthorTests;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;
using Serilog;
using SimpleE2ETesterLibrary.Extensions.Responses;
using SimpleE2ETesterLibrary.Extensions.SimpleWork;
using SimpleE2ETesterLibrary.Extensions.Tester;

namespace BookLovers.Readers.Tests.EndToEndTests.AddReviewTests
{
    public class AddReviewTest : EndToEndTest
    {
        private JsonWebToken _token;
        private string _registrationToken;
        private Guid _userGuid;
        private Guid _authorGuid;
        private Guid _seriesGuid;
        private Guid _publisherGuid;
        private Guid _bookGuid;
        private Guid _reviewGuid;

        [Test]
        public async Task AddReview_WhenCalled_ShouldReturnResponseWithCreatedStatusCode()
        {
            IModule<PublicationModule> module = new PublicationModule();
            var poller = new Poller(TimeChecker.WithSeconds(30));

            await E2ETester
                .ArePrecedingRequestsSuccessful((flag) => flag.Should().BeTrue())
                .SendAsyncAndMapToResult(() => AddReview())
                .Tap((result) => result.IsCreated().Should().BeTrue())
                .Back()
                .AddTask(async () => await poller.Check(new AddFavouriteAuthorPublicationModuleProbe(module)))
                .DoAsync();
        }

        protected override IKernel CreateKernel()
        {
            var root = new StandardKernel(
                new CacheModule(),
                new ApiModule());

            RegisterServices(root);

            InitializeModules(root.Get<IHttpContextAccessor>(), root.Get<IAppManager>());

            return root;
        }

        protected override void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IModule<AuthModule>>().To<AuthModule>();
            kernel.Bind<IModule<PublicationModule>>().To<PublicationModule>();
            kernel.Bind<IModule<LibrarianModule>>().To<LibrarianModule>();
            kernel.Bind<IModule<ReadersModule>>().To<ReadersModule>();

            _userGuid = Fixture.Create<Guid>();
            var httpContextAccessor = new FakeHttpContextAccessor(_userGuid, true);

            var appManagerMock = new Mock<IAppManager>();
            appManagerMock.Setup(s => s.GetConfigValue(JwtSettings.Issuer)).Returns("http://localhost:64892/");
            appManagerMock.Setup(s => s.GetConfigValue(JwtSettings.JsonWebTokenKey)).Returns("mySuperSecretKey");

            var authConnectionString = Environment.GetEnvironmentVariable(AuthContext.ConnectionStringKey);
            if (authConnectionString.IsEmpty())
                authConnectionString = E2EConstants.AuthConnectionString;

            var publicationsConnectionString =
                Environment.GetEnvironmentVariable(PublicationsContext.ConnectionStringKey);
            if (publicationsConnectionString.IsEmpty())
                publicationsConnectionString = E2EConstants.PublicationsConnectionString;

            var publicationsStoreConnectionString =
                Environment.GetEnvironmentVariable(PublicationsStoreContext.ConnectionStringKey);
            if (publicationsStoreConnectionString.IsEmpty())
                publicationsStoreConnectionString = E2EConstants.PublicationsStoreConnectionString;

            var librariansConnectionString = Environment.GetEnvironmentVariable(LibrariansContext.ConnectionStringKey);
            if (librariansConnectionString.IsEmpty())
                librariansConnectionString = E2EConstants.LibrariansConnectionString;

            var readersConnectionString = Environment.GetEnvironmentVariable(ReadersContext.ConnectionStringKey);
            if (readersConnectionString.IsEmpty())
                readersConnectionString = E2EConstants.ReadersConnectionString;

            var readersStoreConnectionString =
                Environment.GetEnvironmentVariable(ReadersStoreContext.ConnectionStringKey);
            if (readersStoreConnectionString.IsEmpty())
                readersStoreConnectionString = E2EConstants.ReadersStoreConnectionString;

            appManagerMock.Setup(s => s.GetConfigValue(AuthContext.ConnectionStringKey))
                .Returns(authConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(PublicationsContext.ConnectionStringKey))
                .Returns(publicationsConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(PublicationsStoreContext.ConnectionStringKey))
                .Returns(publicationsStoreConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(LibrariansContext.ConnectionStringKey))
                .Returns(librariansConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(ReadersContext.ConnectionStringKey))
                .Returns(readersConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(ReadersStoreContext.ConnectionStringKey))
                .Returns(readersStoreConnectionString);

            kernel.Bind<IHttpContextAccessor>().ToConstant(httpContextAccessor);
            kernel.Bind<IAppManager>().ToConstant(appManagerMock.Object);
            kernel.Bind<ITokenManager>().To<TokenManager>();
        }

        protected override void InitializeModules(
            IHttpContextAccessor contextAccessor,
            IAppManager manager)
        {
            AuthModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
            ReadersModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
            PublicationModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
            LibrarianModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
        }

        protected override async Task SendPreRequestsAsync()
        {
            await E2ETester
                .Act(() => this._bookGuid = Fixture.Create<Guid>())
                .Act(() => this._authorGuid = Fixture.Create<Guid>())
                .Act(() => this._seriesGuid = Fixture.Create<Guid>())
                .Act(() => this._publisherGuid = Fixture.Create<Guid>())
                .Act(() => this._reviewGuid = Fixture.Create<Guid>())
                .SendAsync(RegisterUser)
                .Map(() => this.Kernel.Get<IModule<AuthModule>>())
                .Tap(async module => await module.SendCommandAsync(CreateSuperAdminCommand.Create()))
                .Back()
                .SendSynchronouslyAndMapToResult(new GetRegistrationSummaryHttpRequest(E2EConstants.DefaultUserEmail))
                .Tap(async (response) => _registrationToken = await response.GetResponseContentAsync<string>())
                .Back()
                .SendAsync(new CompleteRegistrationHttpRequest(E2EConstants.DefaultUserEmail, _registrationToken))
                .SendSynchronouslyAndMapToResult(LoginAsSuperAdmin)
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>())
                .Back()
                .SendAsync(CreateLibrarian)
                .SendSynchronouslyAndMapToResult(LoginAsUser)
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>())
                .Back()
                .AddRequestAsync(CreateAuthor)
                .AddRequestAsync(CreatePublisher)
                .AddRequestAsync(CreateSeries)
                .SendPendingRequestsAsync()
                .SendAsync(CreateBook);
        }

        private AddReviewHttpRequest AddReview()
        {
            return new AddReviewHttpRequest(_reviewGuid, _bookGuid, _token);
        }

        protected override void ConfigureLogger()
        {
            Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo
                .Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        private CreateBookHttpRequest CreateBook()
        {
            var authors = new List<Guid>();
            authors.Add(_authorGuid);

            return new CreateBookHttpRequest(_publisherGuid, _seriesGuid,
                    _userGuid, authors, _token)
                .WithBookGuid(_bookGuid);
        }

        private CreatePublisherHttpRequest CreatePublisher()
        {
            return new CreatePublisherHttpRequest(_token)
                .WithPublisherGuid(_publisherGuid);
        }

        private CreateSeriesHttpRequest CreateSeries()
        {
            return new CreateSeriesHttpRequest(_token)
                .WithSeriesGuid(_seriesGuid);
        }

        private CreateAuthorHttpRequest CreateAuthor()
        {
            return new CreateAuthorHttpRequest(_userGuid, _token)
                .AddCommandData().WithAuthorGuid(_authorGuid);
        }

        private LoginUserRequest LoginAsUser()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.DefaultUserEmail)
                .WithPassword(E2EConstants.DefaultPassword)
                .WithClientId(E2EConstants.AudienceGuid)
                .WithClientSecret(E2EConstants.ClientSecret);
        }

        private CreateLibrarianHttpRequest CreateLibrarian()
        {
            return new CreateLibrarianHttpRequest(_token).WithReaderGuid(_userGuid);
        }

        private LoginUserRequest LoginAsSuperAdmin()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.SuperAdminEmail)
                .WithPassword(E2EConstants.DefaultPassword)
                .WithClientId(E2EConstants.AudienceGuid)
                .WithClientSecret(E2EConstants.ClientSecret);
        }

        private RegisterUserHttpRequest RegisterUser()
        {
            return new RegisterUserHttpRequest()
                .WithUserGuid(_userGuid)
                .WithEmailAndUserName(E2EConstants.DefaultUserEmail, E2EConstants.DefaultUserNickName)
                .WithPassword(E2EConstants.DefaultPassword);
        }
    }
}