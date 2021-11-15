using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.HttpRequests.Auth;
using BaseTests.EndToEndHelpers.HttpRequests.Bookcase;
using BaseTests.EndToEndHelpers.HttpRequests.Librarians;
using BaseTests.EndToEndHelpers.HttpRequests.Publications;
using BaseTests.EndToEndHelpers.HttpRequests.Ratings;
using BaseTests.EndToEndHelpers.HttpRequests.Readers;
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
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Root;
using BookLovers.Bookcases.Store.Persistence;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Root;
using BookLovers.Modules;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Ratings.Domain;
using BookLovers.Ratings.Domain.RatingStars;
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
using ReaderDto = BookLovers.Readers.Infrastructure.Dtos.Readers.ReaderDto;

namespace BookLovers.Ratings.Tests.EndToEndTests
{
    public class ChangeRatingAndBookDoesNotExist : EndToEndTest
    {
        private string _registrationToken;
        private Guid _userGuid;
        private Guid _bookGuid;
        private Guid _authorGuid;
        private Guid _publisherGuid;
        private Guid _seriesGuid;
        private Guid _bookcaseGuid;
        private JsonWebToken _token;
        private ReaderDto _readerDto;
        private BookcaseDto _bookcaseDto;

        [Test]
        public async Task ChangeRating_WhenCalledAndBookDoesNotExist_ShouldReturnResponseWithConflictStatusCode()
        {
            await this.E2ETester
                .ArePrecedingRequestsSuccessful(flag => flag.Should().BeTrue())
                .SendAsyncAndMapToResult(() => new ChangeRatingHttpRequest(Star.Two, this.Fixture.Create<Guid>(),
                    this._token))
                .Tap(result => result.IsConflict().Should().BeTrue());
        }

        protected override IKernel CreateKernel()
        {
            var root = new StandardKernel(
                new ApiModule(),
                new CacheModule());

            this.RegisterServices(root);

            this.InitializeModules(root.Get<IHttpContextAccessor>(), root.Get<IAppManager>());

            return root;
        }

        protected override void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IModule<AuthModule>>().To<AuthModule>();
            kernel.Bind<IModule<BookcaseModule>>().To<BookcaseModule>();
            kernel.Bind<IModule<PublicationModule>>().To<PublicationModule>();
            kernel.Bind<IModule<LibrarianModule>>().To<LibrarianModule>();
            kernel.Bind<IModule<RatingsModule>>().To<RatingsModule>();
            kernel.Bind<IModule<ReadersModule>>().To<ReadersModule>();

            this._userGuid = this.Fixture.Create<Guid>();
            var httpContextAccessor = new FakeHttpContextAccessor(this._userGuid, true);

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
            AuthModuleStartup.Initialize(contextAccessor, manager, this.Logger, PersistenceSettings.Default());
            LibrarianModuleStartup.Initialize(contextAccessor, manager, this.Logger, PersistenceSettings.Default());
            PublicationModuleStartup.Initialize(contextAccessor, manager, this.Logger, PersistenceSettings.Default());
            RatingsModuleStartup.Initialize(contextAccessor, manager, this.Logger, PersistenceSettings.Default());
            ReadersModuleStartup.Initialize(contextAccessor, manager, this.Logger, PersistenceSettings.Default());
            BookcaseModuleStartup.Initialize(contextAccessor, manager, this.Logger, PersistenceSettings.Default());
        }

        protected override async Task SendPreRequestsAsync()
        {
            await this.E2ETester
                .Act(() => this._bookGuid = Guid.NewGuid())
                .Act(() => this._authorGuid = Guid.NewGuid())
                .Act(() => this._publisherGuid = Guid.NewGuid())
                .Act(() => this._seriesGuid = Guid.NewGuid())
                .Act(() => this._bookcaseGuid = Guid.NewGuid())
                .Act(() => RatingsModuleStartup.AddOrUpdateService(
                    new FakeBookInBookcaseChecker(this.TestServer.HttpClient), true))
                .SendAsync(this.RegisterUser)
                .Map(() => this.Kernel.Get<IModule<AuthModule>>())
                .Tap(async module => await module.SendCommandAsync(CreateSuperAdminCommand.Create()))
                .Back()
                .SendSynchronouslyAndMapToResult(new GetRegistrationSummaryHttpRequest(E2EConstants.DefaultUserEmail))
                .Tap(async (response) => _registrationToken = await response.GetResponseContentAsync<string>())
                .Back()
                .SendAsync(() => new CompleteRegistrationHttpRequest(
                    E2EConstants.DefaultUserEmail,
                    this._registrationToken))
                .SendSynchronouslyAndMapToResult(this.LoginAsSuperAdmin)
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>())
                .Back()
                .SendAsync(this.CreateLibrarian)
                .SendSynchronouslyAndMapToResult(this.LoginAsUser)
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>())
                .Back()
                .AddRequestAsync(this.CreateAuthor)
                .AddRequestAsync(this.CreatePublisher)
                .AddRequestAsync(this.CreateSeries)
                .SendPendingRequestsAsync()
                .SendAsync(this.CreateBook)
                .SendSynchronouslyAndMapToResult(new GetReaderByGuidHttpRequest(this._userGuid))
                .Tap(async (response) => _readerDto = await response.GetResponseContentAsync<ReaderDto>())
                .Back()
                .SendSynchronouslyAndMapToResult(this.LoginAsUser)
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>())
                .Back()
                .SendSynchronouslyAndMapToResult(new GetBookcaseByReaderIdHttpRequest(
                    this._readerDto.ReaderId,
                    this._token.AccessToken))
                .Tap(async (response) => _bookcaseDto = await response.GetResponseContentAsync<BookcaseDto>())
                .Back()
                .SendAsync(new AddToShelfHttpRequest(
                    this._bookcaseGuid,
                    this._bookcaseDto.Shelves.First().Guid, this._bookGuid, this._token))
                .Act(() => RatingsModuleStartup.AddOrUpdateService<IBookInBookcaseChecker>(
                    new FakeBookInBookcaseChecker(this.TestServer.HttpClient), true))
                .SendAsync(() => new AddRatingHttpRequest(Star.Five, this._bookGuid, this._token));
        }

        protected override void ConfigureLogger()
        {
            this.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo
                .Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        private LoginUserRequest LoginAsSuperAdmin()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.SuperAdminEmail)
                .WithPassword(E2EConstants.DefaultPassword)
                .WithClientId(E2EConstants.AudienceGuid)
                .WithClientSecret(E2EConstants.ClientSecret);
        }

        private CreateBookHttpRequest CreateBook()
        {
            var authors = new List<Guid> { this._authorGuid };
            return new CreateBookHttpRequest(_publisherGuid, _seriesGuid, _userGuid,
                    authors, _token)
                .WithBookGuid(this._bookGuid);
        }

        private CreateSeriesHttpRequest CreateSeries()
        {
            return new CreateSeriesHttpRequest(this._token)
                .WithSeriesGuid(this._seriesGuid);
        }

        private CreatePublisherHttpRequest CreatePublisher()
        {
            return new CreatePublisherHttpRequest(this._token)
                .WithPublisherGuid(this._publisherGuid);
        }

        private CreateAuthorHttpRequest CreateAuthor()
        {
            return new CreateAuthorHttpRequest(this._userGuid, this._token)
                .AddCommandData()
                .WithAuthorGuid(this._authorGuid);
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
            return new CreateLibrarianHttpRequest(this._token)
                .WithReaderGuid(this._userGuid);
        }

        private RegisterUserHttpRequest RegisterUser()
        {
            return new RegisterUserHttpRequest()
                .WithUserGuid(this._userGuid)
                .WithBookcaseGuid(this._bookcaseGuid)
                .WithEmailAndUserName(E2EConstants.DefaultUserEmail, E2EConstants.DefaultUserNickName)
                .WithPassword(E2EConstants.DefaultPassword);
        }
    }
}