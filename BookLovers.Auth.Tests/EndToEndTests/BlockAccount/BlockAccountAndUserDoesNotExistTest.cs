using System;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.HttpRequests.Auth;
using BaseTests.EndToEndHelpers.HttpRequests.Librarians;
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
using Serilog.Exceptions;
using SimpleE2ETesterLibrary.Extensions.Responses;
using SimpleE2ETesterLibrary.Extensions.SimpleWork;
using SimpleE2ETesterLibrary.Extensions.Tester;

namespace BookLovers.Auth.Tests.EndToEndTests.BlockAccount
{
    public class BlockAccountAndUserDoesNotExistTest : EndToEndTest
    {
        private Guid _userGuid;

        private string _firstRegistrationToken;
        private string _secondRegistrationToken;

        private JsonWebToken _token;
        private Guid _userToBlockGuid;

        [Test]
        public async Task BlockAccount_WhenCalled_ShouldReturnOkResponseWithConflictStatusCode()
        {
            await this.E2ETester
                .ArePrecedingRequestsSuccessful(flag => flag.Should().BeTrue())
                .SendAsyncAndMapToResult(
                    new BlockAccountHttpRequest(
                        Fixture.Create<Guid>(),
                        this._token))
                .Tap(result => result.IsConflict().Should().BeTrue());
        }

        protected override IKernel CreateKernel()
        {
            var root = new StandardKernel(new CacheModule());

            RegisterServices(root);

            var httpContextAccessor = root.Get<IHttpContextAccessor>();
            var appManager = root.Get<IAppManager>();

            InitializeModules(httpContextAccessor, appManager);

            return root;
        }

        protected override void RegisterServices(IKernel kernel)
        {
            _userGuid = Fixture.Create<Guid>();
            _userToBlockGuid = Fixture.Create<Guid>();

            kernel.Bind<IModule<AuthModule>>().To<AuthModule>();
            kernel.Bind<IModule<LibrarianModule>>().To<LibrarianModule>();
            kernel.Bind<IModule<ReadersModule>>().To<ReadersModule>();
            kernel.Bind<IModule<PublicationModule>>().To<PublicationModule>();
            kernel.Bind<IModule<BookcaseModule>>().To<BookcaseModule>();
            kernel.Bind<IModule<RatingsModule>>().To<RatingsModule>();

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

            var bookcaseStoreConnectionString = Environment.GetEnvironmentVariable(BookcaseStoreContext.ConnectionStringKey);
            if (bookcaseStoreConnectionString.IsEmpty())
                bookcaseStoreConnectionString = E2EConstants.BookcaseStoreConnectionString;
            
            var publicationsConnectionString = Environment.GetEnvironmentVariable(PublicationsContext.ConnectionStringKey);
            if (publicationsConnectionString.IsEmpty())
                publicationsConnectionString = E2EConstants.PublicationsConnectionString;
            
            var publicationsStoreConnectionString = Environment.GetEnvironmentVariable(PublicationsStoreContext.ConnectionStringKey);
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
            
            var readersStoreConnectionString = Environment.GetEnvironmentVariable(ReadersStoreContext.ConnectionStringKey);
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
            await this.E2ETester
                .AddRequest(RegisterFirstUser)
                .AddRequest(RegisterSecondUser)
                .SendPendingRequestsAsync()
                .Map(() => this.Kernel.Get<IModule<AuthModule>>())
                .Tap(async (module) => await module.SendCommandAsync(CreateSuperAdminCommand.Create()))
                .Back()
                .SendMultipleSynchronously(
                    new GetRegistrationSummaryHttpRequest(E2EConstants.DefaultUserEmail),
                    new GetRegistrationSummaryHttpRequest(E2EConstants.UserToBlockEmail))
                .TapMultiple(
                    async (response) => this._firstRegistrationToken =
                        await response.GetResponseContentAsync<string>(),
                    async (response) => this._secondRegistrationToken =
                        await response.GetResponseContentAsync<string>())
                .Back()
                .AddRequestAsync(() =>
                    new CompleteRegistrationHttpRequest(E2EConstants.DefaultUserEmail, _firstRegistrationToken))
                .AddRequestAsync(() =>
                    new CompleteRegistrationHttpRequest(E2EConstants.UserToBlockEmail, _secondRegistrationToken))
                .SendPendingRequestsAsync()
                .SendSynchronouslyAndMapToResult(() => LoginAsSuperAdmin())
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>())
                .Back()
                .SendAsync(() => new CreateLibrarianHttpRequest(_token).WithReaderGuid(_userGuid))
                .SendSynchronouslyAndMapToResult(() => LoginAsUser())
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>())
                .Back();
        }

        protected override void ConfigureLogger()
        {
            Logger = new LoggerConfiguration().Enrich.WithExceptionDetails()
                .MinimumLevel.Debug().WriteTo
                .Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        private RegisterUserHttpRequest RegisterSecondUser()
        {
            return new RegisterUserHttpRequest()
                .WithUserGuid(_userToBlockGuid)
                .WithEmailAndUserName(E2EConstants.UserToBlockEmail, E2EConstants.UserToBlockNickName)
                .WithPassword(E2EConstants.DefaultPassword);
        }

        private RegisterUserHttpRequest RegisterFirstUser()
        {
            return new RegisterUserHttpRequest()
                .WithUserGuid(_userGuid)
                .WithEmailAndUserName(E2EConstants.DefaultUserEmail, E2EConstants.DefaultUserNickName)
                .WithPassword(E2EConstants.DefaultPassword);
        }

        private LoginUserRequest LoginAsSecondUser()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.UserToBlockEmail)
                .WithPassword(E2EConstants.DefaultPassword)
                .WithClientId(E2EConstants.AudienceGuid)
                .WithClientSecret(E2EConstants.ClientSecret);
        }

        private LoginUserRequest LoginAsUser()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.DefaultUserEmail)
                .WithPassword(E2EConstants.DefaultPassword)
                .WithClientId(E2EConstants.AudienceGuid)
                .WithClientSecret(E2EConstants.ClientSecret);
        }

        private LoginUserRequest LoginAsSuperAdmin()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.SuperAdminEmail)
                .WithPassword(E2EConstants.DefaultPassword)
                .WithClientId(E2EConstants.AudienceGuid)
                .WithClientSecret(E2EConstants.ClientSecret);
        }
    }
}