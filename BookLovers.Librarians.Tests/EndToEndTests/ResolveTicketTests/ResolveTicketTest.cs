using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests.DataCreationHelpers;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.HttpRequests.Auth;
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
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Root;
using BookLovers.Modules;
using BookLovers.Publication.Application.WriteModels.Books;
using BookLovers.Publication.Domain.Books.CoverTypes;
using BookLovers.Publication.Domain.Books.Languages;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Root;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Readers.Store.Persistence;
using BookLovers.Shared.Categories;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Ninject;
using NUnit.Framework;
using Serilog;
using Serilog.Exceptions;
using SimpleE2ETesterLibrary.Extensions.Responses;
using SimpleE2ETesterLibrary.Extensions.SimpleWork;
using SimpleE2ETesterLibrary.Extensions.Tester;

namespace BookLovers.Librarians.Tests.EndToEndTests.ResolveTicketTests
{
    public class ResolveTicketTest : EndToEndTest
    {
        private string _firstRegistrationToken;
        private string _secondRegistrationToken;
        private JsonWebToken _token;
        private Guid _userGuid;
        private Guid _secondUserGuid;
        private Guid _bookGuid;
        private Guid _authorGuid;
        private Guid _publisherGuid;
        private Guid _seriesGuid;
        private Guid _ticketGuid;
        private Guid _librarianGuid;

        private FakeHttpContextAccessor _firstUserHttpContextAccessor;
        private FakeHttpContextAccessor _secondUserHttpContextAccessor;

        [Test]
        public async Task ResolveTicket_WhenCalled_ShouldReturnResponseWithOkStatusCode()
        {
            IModule<PublicationModule> booksModule = new PublicationModule();
            IModule<ReadersModule> readersModule = new ReadersModule();
            IModule<BookcaseModule> bookcaseModule = new BookcaseModule();
            var poller = new Poller(TimeChecker.WithSeconds(20));

            await this.E2ETester
                .ArePrecedingRequestsSuccessful((flag) => flag.Should().BeTrue())
                .SendAsyncAndMapToResult(() =>
                    new ResolveTicketHttpRequest(_ticketGuid, _librarianGuid, Decision.Approve, TicketConcern.Book,
                        _token))
                .Tap((result) => result.IsOk().Should().BeTrue())
                .Back()
                .AddTask(
                    async () => await poller.Check(new ResolveTicketPublicationModuleProbe(booksModule, _bookGuid)))
                .AddTask(async () => await poller.Check(new ResolveTicketBookcaseModuleProbe(bookcaseModule)))
                .AddTask(async () => await poller.Check(new ResolveTicketReadersModuleProbe(readersModule)))
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
            kernel.Bind<IModule<BookcaseModule>>().To<BookcaseModule>();
            kernel.Bind<IModule<LibrarianModule>>().To<LibrarianModule>();
            kernel.Bind<IModule<ReadersModule>>().To<ReadersModule>();
            kernel.Bind<IModule<RatingsModule>>().To<RatingsModule>();
            kernel.Bind<IModule<PublicationModule>>().To<PublicationModule>();

            _userGuid = Fixture.Create<Guid>();
            _secondUserGuid = Fixture.Create<Guid>();
            _firstUserHttpContextAccessor = new FakeHttpContextAccessor(_userGuid, true);
            _secondUserHttpContextAccessor = new FakeHttpContextAccessor(_secondUserGuid, true);

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
            

            var readersConnectionString = Environment.GetEnvironmentVariable(ReadersContext.ConnectionStringKey);
            if (readersConnectionString.IsEmpty())
                readersConnectionString = E2EConstants.ReadersConnectionString;

            var ratingsConnectionString = Environment.GetEnvironmentVariable(RatingsContext.ConnectionStringKey);
            if (ratingsConnectionString.IsEmpty())
                ratingsConnectionString = E2EConstants.RatingsConnectionString;
            
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

            kernel.Bind<IHttpContextAccessor>().ToConstant(_firstUserHttpContextAccessor);
            kernel.Bind<IAppManager>().ToConstant(appManagerMock.Object);
            kernel.Bind<ITokenManager>().To<TokenManager>();
        }

        protected override void InitializeModules(
            IHttpContextAccessor contextAccessor,
            IAppManager manager)
        {
            AuthModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
            LibrarianModuleStartup.Initialize(_secondUserHttpContextAccessor, manager, Logger,
                PersistenceSettings.Default());
            PublicationModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
            RatingsModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
            ReadersModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
            BookcaseModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
        }

        protected override async Task SendPreRequestsAsync()
        {
            await this.E2ETester
                .Act(() => this._bookGuid = Fixture.Create<Guid>())
                .Act(() => this._authorGuid = Fixture.Create<Guid>())
                .Act(() => this._publisherGuid = Fixture.Create<Guid>())
                .Act(() => this._seriesGuid = Fixture.Create<Guid>())
                .Act(() => this._librarianGuid = Fixture.Create<Guid>())
                .Act(() => this._ticketGuid = Fixture.Create<Guid>())
                .AddRequest(RegisterUser)
                .AddRequest(RegisterSecondUser)
                .SendPendingRequestsAsync()
                .Map(() => this.Kernel.Get<IModule<AuthModule>>())
                .Tap(async module => await module.SendCommandAsync(CreateSuperAdminCommand.Create()))
                .Back()
                .SendMultipleSynchronously(
                    new GetRegistrationSummaryHttpRequest(E2EConstants.DefaultUserEmail),
                    new GetRegistrationSummaryHttpRequest(E2EConstants.SecondUserEmail))
                .TapMultiple(
                    async (response) => _firstRegistrationToken = await response.GetResponseContentAsync<string>(),
                    async (response) => _secondRegistrationToken = await response.GetResponseContentAsync<string>())
                .Back()
                .AddRequestAsync(
                    new CompleteRegistrationHttpRequest(E2EConstants.DefaultUserEmail, _firstRegistrationToken))
                .AddRequestAsync(
                    new CompleteRegistrationHttpRequest(E2EConstants.SecondUserEmail, _secondRegistrationToken))
                .SendPendingRequestsAsync()
                .SendSynchronouslyAndMapToResult(LoginAsAdmin)
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>())
                .Back()
                .SendAsync(new CreateLibrarianHttpRequest(_token).WithReaderGuid(_userGuid)
                    .WithLibrarianGuid(_librarianGuid))
                .SendSynchronouslyAndMapToResult(LoginAsLibrarian)
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>())
                .Back()
                .AddRequestAsync(CreateAuthor)
                .AddRequestAsync(CreatePublisher)
                .AddRequestAsync(CreateSeries)
                .SendPendingRequestsAsync()
                .SendAsync(() => new CreateTicketHttpRequest(_token)
                    .WithTicketConcern(TicketConcern.Book)
                    .WithTicketGuid(_ticketGuid)
                    .WithTicketData(_bookGuid, GetTicketData()));
        }

        protected override void ConfigureLogger()
        {
            Logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Debug().WriteTo
                .Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        private string GetTicketData()
        {
            var bookWriteModel = Fixture.Build<BookWriteModel>()
                .With(p => p.BookGuid, _bookGuid)
                .With(p => p.Description)
                .WithBookBasics(Category.Fiction, SubCategory.FictionSubCategory.Action, "9788375155280",
                    Fixture.Create<string>(), _publisherGuid).WithDetails(Fixture.Create<int>(), Language.English)
                .WithCover(CoverType.PaperBack, false).WithSeries(_seriesGuid, 1).With(p => p.AddedBy, _secondUserGuid)
                .With(p => p.Authors, new List<Guid>()
                {
                    _authorGuid
                })
                .WithCycles(new List<Guid>())
                .Create();

            var model = Fixture.Build<CreateBookWriteModel>()
                .With(p => p.BookWriteModel, bookWriteModel)
                .With(p => p.PictureWriteModel, new BookPictureWriteModel()
                {
                    Cover = string.Empty,
                    FileName = string.Empty
                }).Create();

            return JsonConvert.SerializeObject(model);
        }

        private CreateBookHttpRequest CreateBook()
        {
            var authors = new List<Guid> { _authorGuid };

            return new CreateBookHttpRequest(_publisherGuid, _seriesGuid,
                    _userGuid, authors, _token)
                .WithBookGuid(_bookGuid);
        }

        private CreateSeriesHttpRequest CreateSeries()
        {
            return new CreateSeriesHttpRequest(_token)
                .WithSeriesGuid(_seriesGuid);
        }

        private CreatePublisherHttpRequest CreatePublisher()
        {
            return new CreatePublisherHttpRequest(_token)
                .WithPublisherGuid(_publisherGuid);
        }

        private CreateAuthorHttpRequest CreateAuthor()
        {
            return new CreateAuthorHttpRequest(_userGuid, _token)
                .AddCommandData()
                .WithAuthorGuid(_authorGuid);
        }

        private LoginUserRequest LoginAsLibrarian()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.DefaultUserEmail)
                .WithPassword(E2EConstants.DefaultPassword)
                .WithClientId(E2EConstants.AudienceGuid)
                .WithClientSecret(E2EConstants.ClientSecret);
        }

        private LoginUserRequest LoginAsAdmin()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.SuperAdminEmail)
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

        private RegisterUserHttpRequest RegisterUser()
        {
            return new RegisterUserHttpRequest()
                .WithUserGuid(_userGuid)
                .WithEmailAndUserName(E2EConstants.DefaultUserEmail, E2EConstants.DefaultUserNickName)
                .WithPassword(E2EConstants.DefaultPassword);
        }
    }
}