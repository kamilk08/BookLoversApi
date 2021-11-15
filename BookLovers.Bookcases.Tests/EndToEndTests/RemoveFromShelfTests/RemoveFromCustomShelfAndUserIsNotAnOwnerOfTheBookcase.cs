using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.HttpRequests.Auth;
using BaseTests.EndToEndHelpers.HttpRequests.Bookcase;
using BaseTests.EndToEndHelpers.HttpRequests.Librarians;
using BaseTests.EndToEndHelpers.HttpRequests.Publications;
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
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Root;
using BookLovers.Bookcases.Store.Persistence;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Root;
using BookLovers.Modules;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Root;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Readers.Store.Persistence;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;
using Serilog;
using SimpleE2ETesterLibrary.Extensions.Responses;
using SimpleE2ETesterLibrary.Extensions.SimpleWork;
using SimpleE2ETesterLibrary.Extensions.Tester;

namespace BookLovers.Bookcases.Tests.EndToEndTests.RemoveFromShelfTests
{
    public class RemoveFromCustomShelfAndUserIsNotAnOwnerOfTheBookcaseTest : EndToEndTest
    {
        private string _firstRegistrationToken;
        private string _secondRegistrationToken;
        private Guid _bookGuid;
        private Guid _publisherGuid;
        private Guid _seriesGuid;
        private Guid _authorGuid;
        private Guid _userGuid;
        private Guid _secondUserGuid;
        private Guid _bookcaseGuid;
        private Guid _shelfGuid;
        private JsonWebToken _token;

        [Test]
        public async Task
            RemoveFromShelf_WhenCalledAndUserIsNotAnOwnerOfTheBookcase_ShouldReturnResponseWithForbiddenStatusCode()
        {
            await E2ETester.SendAsyncAndMapToResult(() =>
                    new RemoveFromShelfRequest(_bookcaseGuid, _shelfGuid, _bookGuid, _token))
                .Tap((result) => result.IsForbidden().Should().BeTrue());
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
            kernel.Bind<IModule<BookcaseModule>>().To<BookcaseModule>();
            kernel.Bind<IModule<LibrarianModule>>().To<LibrarianModule>();
            kernel.Bind<IModule<PublicationModule>>().To<PublicationModule>();
            kernel.Bind<IModule<ReadersModule>>().To<ReadersModule>();
            kernel.Bind<IModule<RatingsModule>>().To<RatingsModule>();

            _userGuid = Fixture.Create<Guid>();
            _secondUserGuid = Fixture.Create<Guid>();
            var httpContextAccessor = new FakeHttpContextAccessor(_userGuid, true);

            var appManagerMock = new Mock<IAppManager>();

            appManagerMock.Setup(s => s.GetConfigValue(JwtSettings.Issuer)).Returns("http://localhost:64892/");
            appManagerMock.Setup(s => s.GetConfigValue(JwtSettings.JsonWebTokenKey)).Returns("mySuperSecretKey");

            var authConnectionString = Environment.GetEnvironmentVariable(AuthContext.ConnectionStringKey);
            if (authConnectionString.IsEmpty())
                authConnectionString = E2EConstants.AuthConnectionString;

            var bookcaseConnectionString = Environment.GetEnvironmentVariable(BookcaseContext.ConnectionStringKey);
            if (bookcaseConnectionString.IsEmpty())
                bookcaseConnectionString = E2EConstants.BookcaseConnectionString;

            var bookcaseStoreConnectionString =
                Environment.GetEnvironmentVariable(BookcaseStoreContext.ConnectionStringKey);
            if (bookcaseStoreConnectionString.IsEmpty())
                bookcaseStoreConnectionString = E2EConstants.BookcaseStoreConnectionString;

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

            var ratingsConnectionString = Environment.GetEnvironmentVariable(RatingsContext.ConnectionStringKey);
            if (ratingsConnectionString.IsEmpty())
                ratingsConnectionString = E2EConstants.RatingsConnectionString;

            var readersConnectionString = Environment.GetEnvironmentVariable(ReadersContext.ConnectionStringKey);
            if (readersConnectionString.IsEmpty())
                readersConnectionString = E2EConstants.ReadersConnectionString;

            var readersStoreConnectionString =
                Environment.GetEnvironmentVariable(ReadersStoreContext.ConnectionStringKey);
            if (readersStoreConnectionString.IsEmpty())
                readersStoreConnectionString = E2EConstants.ReadersStoreConnectionString;

            appManagerMock.Setup(s => s.GetConfigValue(AuthContext.ConnectionStringKey))
                .Returns(authConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(BookcaseContext.ConnectionStringKey))
                .Returns(bookcaseConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(BookcaseStoreContext.ConnectionStringKey))
                .Returns(bookcaseStoreConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(PublicationsContext.ConnectionStringKey))
                .Returns(publicationsConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(PublicationsStoreContext.ConnectionStringKey))
                .Returns(publicationsStoreConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(LibrariansContext.ConnectionStringKey))
                .Returns(librariansConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(RatingsContext.ConnectionStringKey))
                .Returns(ratingsConnectionString);

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
            LibrarianModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
            PublicationModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
            RatingsModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
            ReadersModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
            BookcaseModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
        }

        protected override async Task SendPreRequestsAsync()
        {
            await E2ETester
                .Act(() => _bookcaseGuid = Fixture.Create<Guid>())
                .Act(() => _authorGuid = Fixture.Create<Guid>())
                .Act(() => _bookGuid = Fixture.Create<Guid>())
                .Act(() => _seriesGuid = Fixture.Create<Guid>())
                .Act(() => _publisherGuid = Fixture.Create<Guid>())
                .Act(() => _shelfGuid = Fixture.Create<Guid>())
                .AddRequest(RegisterFirstUser)
                .AddRequest(RegisterSecondUser)
                .SendPendingRequestsAsync()
                .Map(() => Kernel.Get<IModule<AuthModule>>())
                .Tap(async (module) => await module.SendCommandAsync(CreateSuperAdminCommand.Create()))
                .Back()
                .SendMultipleSynchronously(
                    new GetRegistrationSummaryHttpRequest(E2EConstants.DefaultUserEmail),
                    new GetRegistrationSummaryHttpRequest(E2EConstants.SecondUserEmail))
                .TapMultiple(
                    async (response) => _firstRegistrationToken = await response.GetResponseContentAsync<string>(),
                    async (response) => _secondRegistrationToken = await response.GetResponseContentAsync<string>())
                .Back()
                .AddRequestAsync(() =>
                    new CompleteRegistrationHttpRequest(E2EConstants.DefaultUserEmail, _firstRegistrationToken))
                .AddRequestAsync(() =>
                    new CompleteRegistrationHttpRequest(E2EConstants.SecondUserEmail, _secondRegistrationToken))
                .SendPendingRequestsAsync()
                .SendSynchronouslyAndMapToResult(LoginAsSuperAdmin)
                .Tap(async (result) => _token = await result.GetResponseContentAsync<JsonWebToken>())
                .Back()
                .SendSynchronously(new CreateLibrarianHttpRequest(_token).WithReaderGuid(_userGuid))
                .SendSynchronouslyAndMapToResult(LoginUser)
                .Tap(async (result) => _token = await result.GetResponseContentAsync<JsonWebToken>())
                .Back()
                .AddRequestAsync(CreateAuthor)
                .AddRequestAsync(CreatePublisher)
                .AddRequestAsync(CreateSeries)
                .SendPendingRequestsAsync()
                .SendAsync(CreateBook)
                .SendSynchronously(new AddShelfHttpRequest(_bookcaseGuid, _token).WithShelfGuid(_shelfGuid))
                .SendSynchronously(new AddToShelfHttpRequest(_bookcaseGuid, _shelfGuid, _bookGuid, _token))
                .ActAsync(async () => await Task.Run(() =>
                {
                    var fakeHttpContextAccessor = new FakeHttpContextAccessor(_secondUserGuid, true, false, false);
                    Kernel.Rebind<IHttpContextAccessor>().ToConstant(fakeHttpContextAccessor);
                }));
        }

        protected override void ConfigureLogger()
        {
            Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        private CreateBookHttpRequest CreateBook()
        {
            return new CreateBookHttpRequest(_publisherGuid, _seriesGuid, _userGuid,
                new List<Guid> { _authorGuid }, _token).WithBookGuid(_bookGuid);
        }

        private CreateSeriesHttpRequest CreateSeries()
        {
            return new CreateSeriesHttpRequest(_token).WithSeriesGuid(_seriesGuid);
        }

        private CreatePublisherHttpRequest CreatePublisher()
        {
            return new CreatePublisherHttpRequest(_token)
                .WithPublisherGuid(_publisherGuid);
        }

        private CreateAuthorHttpRequest CreateAuthor()
        {
            return new CreateAuthorHttpRequest(_userGuid, _token).AddCommandData().WithAuthorGuid(_authorGuid);
        }

        private LoginUserRequest LoginAsSuperAdmin()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.SuperAdminEmail)
                .WithPassword(E2EConstants.DefaultPassword)
                .WithClientId(E2EConstants.AudienceGuid)
                .WithClientSecret(E2EConstants.ClientSecret);
        }

        private LoginUserRequest LoginUser()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.DefaultUserEmail)
                .WithPassword(E2EConstants.DefaultPassword)
                .WithClientId(E2EConstants.AudienceGuid)
                .WithClientSecret(E2EConstants.ClientSecret);
        }

        private RegisterUserHttpRequest RegisterSecondUser()
        {
            return new RegisterUserHttpRequest()
                .WithUserGuid(_secondUserGuid)
                .WithEmailAndUserName(E2EConstants.SecondUserEmail, E2EConstants.SecondUserNickName)
                .WithPassword(E2EConstants.DefaultPassword);
        }

        private RegisterUserHttpRequest RegisterFirstUser()
        {
            return new RegisterUserHttpRequest()
                .WithUserGuid(_userGuid)
                .WithBookcaseGuid(_bookcaseGuid)
                .WithEmailAndUserName(E2EConstants.DefaultUserEmail, E2EConstants.DefaultUserNickName)
                .WithPassword(E2EConstants.DefaultPassword);
        }
    }
}