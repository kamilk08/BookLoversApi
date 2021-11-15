using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.HttpRequests.Auth;
using BaseTests.EndToEndHelpers.HttpRequests.Readers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Root;
using BookLovers.Modules;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
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
using SimpleE2ETesterLibrary.Extensions.Tester;

namespace BookLovers.Readers.Tests.EndToEndTests.TimeLineActivitiesTests
{
    public class ShowTimeLineActivityTest : EndToEndTest
    {
        private JsonWebToken _token;
        private string _followedRegistrationToken;
        private string _followerRegistrationToken;
        private Guid _timeLineObjectGuid;
        private DateTime _occuredAt;
        private Guid _followerGuid;
        private Guid _followedGuid;
        private FakeHttpContextAccessor _httpContext;
        private Mock<IAppManager> _appManagerMock;

        [Test]
        public async Task ShowTimeLineActivity_WhenCalled_ShouldReturnResponseWithOkStatusCode()
        {
            await this.E2ETester
                .ArePrecedingRequestsSuccessful((flag) => flag.Should().BeTrue())
                .SendAsyncAndMapToResult(() =>
                    new ShowTimeLineActivityHttpRequest(_timeLineObjectGuid, _occuredAt, _token))
                .Tap((result) => result.IsOk().Should().BeTrue());
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
            kernel.Bind<IModule<ReadersModule>>().To<ReadersModule>();
            kernel.Bind<IModule<LibrarianModule>>().To<LibrarianModule>();
            kernel.Bind<IModule<PublicationModule>>().To<PublicationModule>();

            _followerGuid = Fixture.Create<Guid>();
            _followedGuid = Fixture.Create<Guid>();

            _httpContext = new FakeHttpContextAccessor(_followerGuid, true);
            _appManagerMock = new Mock<IAppManager>();

            _appManagerMock.Setup(s => s.GetConfigValue(JwtSettings.Issuer)).Returns("http://localhost:64892/");
            _appManagerMock.Setup(s => s.GetConfigValue(JwtSettings.JsonWebTokenKey)).Returns("mySuperSecretKey");

            var authConnectionString = Environment.GetEnvironmentVariable(AuthContext.ConnectionStringKey);
            if (authConnectionString.IsEmpty())
                authConnectionString = E2EConstants.AuthConnectionString;

            var publicationsConnectionString = Environment.GetEnvironmentVariable(PublicationsContext.ConnectionStringKey);
            if (publicationsConnectionString.IsEmpty())
                publicationsConnectionString = E2EConstants.PublicationsConnectionString;
            
            var publicationsStoreConnectionString = Environment.GetEnvironmentVariable(PublicationsStoreContext.ConnectionStringKey);
            if (publicationsStoreConnectionString.IsEmpty())
                publicationsStoreConnectionString = E2EConstants.PublicationsStoreConnectionString;

            var librariansConnectionString = Environment.GetEnvironmentVariable(LibrariansContext.ConnectionStringKey);
            if (librariansConnectionString.IsEmpty())
                librariansConnectionString = E2EConstants.LibrariansConnectionString;

            var readersConnectionString = Environment.GetEnvironmentVariable(ReadersContext.ConnectionStringKey);
            if (readersConnectionString.IsEmpty())
                readersConnectionString = E2EConstants.ReadersConnectionString;
            
            var readersStoreConnectionString = Environment.GetEnvironmentVariable(ReadersStoreContext.ConnectionStringKey);
            if (readersStoreConnectionString.IsEmpty())
                readersStoreConnectionString = E2EConstants.ReadersStoreConnectionString;
            
            _appManagerMock.Setup(s => s.GetConfigValue(AuthContext.ConnectionStringKey))
                .Returns(authConnectionString);
            
            _appManagerMock.Setup(s => s.GetConfigValue(PublicationsContext.ConnectionStringKey))
                .Returns(publicationsConnectionString);

            _appManagerMock.Setup(s => s.GetConfigValue(PublicationsStoreContext.ConnectionStringKey))
                .Returns(publicationsStoreConnectionString);

            _appManagerMock.Setup(s => s.GetConfigValue(LibrariansContext.ConnectionStringKey))
                .Returns(librariansConnectionString);

            _appManagerMock.Setup(s => s.GetConfigValue(ReadersContext.ConnectionStringKey))
                .Returns(readersConnectionString);

            _appManagerMock.Setup(s => s.GetConfigValue(ReadersStoreContext.ConnectionStringKey))
                .Returns(readersStoreConnectionString);

            kernel.Bind<IHttpContextAccessor>().ToConstant(_httpContext);
            kernel.Bind<IAppManager>().ToConstant(_appManagerMock.Object);
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
                .AddRequest(RegisterFollowed)
                .AddRequest(RegisterFollower)
                .SendPendingRequestsAsync()
                .SendMultipleSynchronously(
                    new GetRegistrationSummaryHttpRequest(E2EConstants.DefaultUserEmail),
                    new GetRegistrationSummaryHttpRequest(E2EConstants.FollowerEmail))
                .TapMultiple(
                    async (response) => _followedRegistrationToken = await response.GetResponseContentAsync<string>(),
                    async (response) => _followerRegistrationToken = await response.GetResponseContentAsync<string>())
                .Back()
                .AddRequestAsync(new CompleteRegistrationHttpRequest(
                    E2EConstants.DefaultUserEmail,
                    _followedRegistrationToken))
                .AddRequestAsync(
                    new CompleteRegistrationHttpRequest(E2EConstants.FollowerEmail, _followerRegistrationToken))
                .SendPendingRequestsAsync()
                .SendSynchronouslyAndMapToResult(LoginAsFollower)
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>())
                .Back()
                .SendAsync(new FollowReaderHttpRequest(_followedGuid, _token))
                .SendSynchronouslyAndMapToResult(new GetReaderByGuidHttpRequest(_followedGuid))
                .Tap(async (response) =>
                {
                    var readerDto = await response.GetResponseContentAsync<ReaderDto>();
                    E2ETester.SendSynchronouslyAndMapToResult(new GetReaderTimelineHttpRequest(readerDto.ReaderId))
                        .Tap(async (timelineResponse) =>
                        {
                            var timelineDto = await timelineResponse.GetResponseContentAsync<TimeLineDto>();

                            var pageResultResponse = E2ETester.SendSynchronouslyAndMapToResult(
                                new GetTimelineActivityHttpRequest(
                                    timelineDto.Id,
                                    PaginatedResult.DefaultPage, PaginatedResult.DefaultItemsPerPage, false));

                            var activities = await pageResultResponse
                                .GetResponseContentAsync<PaginatedResult<TimeLineActivityDto>>();

                            _occuredAt = activities.Items.First().Date;
                            _timeLineObjectGuid = activities.Items.First().ActivityObjectGuid;
                        });
                })
                .Back().Delay().Seconds(1)
                .ActAsync(async () => await Task.Run(() =>
                {
                    ReadersModuleStartup.Initialize(
                        new FakeHttpContextAccessor(_followedGuid, true),
                        _appManagerMock.Object, Logger, PersistenceSettings.DoNotCleanContext());
                }))
                .SendAsync(() => new HideActivityHttpRequest(_occuredAt, _timeLineObjectGuid, _token));
        }

        protected override void ConfigureLogger()
        {
            Logger = new LoggerConfiguration().Enrich
                .WithExceptionDetails()
                .MinimumLevel.Debug()
                .WriteTo
                .Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        private RegisterUserHttpRequest RegisterFollower()
        {
            return new RegisterUserHttpRequest()
                .WithUserGuid(_followedGuid)
                .WithEmailAndUserName(E2EConstants.FollowerEmail, E2EConstants.FollowerNickName)
                .WithPassword(E2EConstants.DefaultPassword);
        }

        private RegisterUserHttpRequest RegisterFollowed()
        {
            return new RegisterUserHttpRequest()
                .WithUserGuid(_followerGuid)
                .WithEmailAndUserName(E2EConstants.DefaultUserEmail, E2EConstants.DefaultUserNickName)
                .WithPassword(E2EConstants.DefaultPassword);
        }

        private LoginUserRequest LoginAsFollower()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.FollowerEmail)
                .WithPassword(E2EConstants.DefaultPassword)
                .WithClientId(E2EConstants.AudienceGuid)
                .WithClientSecret(E2EConstants.ClientSecret);
        }

        private LoginUserRequest LoginAsFollowed()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.DefaultUserEmail)
                .WithPassword(E2EConstants.DefaultPassword)
                .WithClientId(E2EConstants.AudienceGuid)
                .WithClientSecret(E2EConstants.ClientSecret);
        }
    }
}