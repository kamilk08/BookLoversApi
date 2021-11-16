using System;
using System.Linq;
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
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Root;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;
using Serilog;
using Serilog.Exceptions;
using SimpleE2ETesterLibrary.Extensions.Responses;
using SimpleE2ETesterLibrary.Extensions.SimpleWork;
using SimpleE2ETesterLibrary.Extensions.Tester;

namespace BookLovers.Auth.Tests.EndToEndTests.RevokeToken
{
    public class RevokeTokenAndTokenHasBeenAlreadyRevoked : EndToEndTest
    {
        private Guid _userGuid;
        private Guid _tokenGuid;
        private JsonWebToken _token;
        private string _registrationToken;

        [Test]
        public async Task
            RevokeToken_WhenCalledAndTokenHasBeenAlreadyRevoked_ShouldReturnResponseWithConflictStatusCode()
        {
            await this.E2ETester
                .ArePrecedingRequestsSuccessful((flag) => flag.Should().BeTrue())
                .SendAsyncAndMapToResult(() => new RevokeTokenHttpRequest(_tokenGuid, _token))
                .Tap((result) => result.IsConflict().Should().BeTrue());
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new BaseModule());

            RegisterServices(kernel);

            var contextAccessor = kernel.Get<IHttpContextAccessor>();
            var configurationManager = kernel.Get<IAppManager>();

            InitializeModules(contextAccessor, configurationManager);

            return kernel;
        }

        protected override void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IModule<AuthModule>>().To<AuthModule>();

            kernel.Bind<IModule<LibrarianModule>>().To<LibrarianModule>();

            kernel.Bind<AuthContext>().ToSelf()
                .WithConstructorArgument(
                    "connectionString",
                    @"Server=localhost\SQLEXPRESS;Database=AuthorizationContext;Trusted_Connection=True;");

            var httpContextAccessor = new FakeHttpContextAccessor(Fixture.Create<Guid>(), true);

            var appManagerMock = new Mock<IAppManager>();

            appManagerMock.Setup(s => s.GetConfigValue(JwtSettings.Issuer))
                .Returns("http://localhost:64892/");

            appManagerMock.Setup(s => s.GetConfigValue(JwtSettings.JsonWebTokenKey))
                .Returns("mySuperSecretKey");

            var authConnectionString = Environment.GetEnvironmentVariable(AuthContext.ConnectionStringKey);
            if (authConnectionString.IsEmpty())
                authConnectionString = E2EConstants.AuthConnectionString;

            var librariansConnectionString = Environment.GetEnvironmentVariable(LibrariansContext.ConnectionStringKey);
            if (librariansConnectionString.IsEmpty())
                librariansConnectionString = E2EConstants.LibrariansConnectionString;

            appManagerMock.Setup(s => s.GetConfigValue(AuthContext.ConnectionStringKey))
                .Returns(authConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(LibrariansContext.ConnectionStringKey))
                .Returns(librariansConnectionString);

            kernel.Bind<IHttpContextAccessor>().ToConstant(httpContextAccessor);
            kernel.Bind<IAppManager>().ToConstant(appManagerMock.Object);
            kernel.Bind<ITokenManager>().To<TokenManager>();
        }

        protected override void InitializeModules(
            IHttpContextAccessor contextAccessor,
            IAppManager manager)
        {
            LibrarianModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
            AuthModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
        }

        protected override async Task SendPreRequestsAsync()
        {
            await this.E2ETester
                .Act(() => _userGuid = Fixture.Create<Guid>())
                .SendAsync(RegisterUser)
                .Map(() => Kernel.Get<IModule<AuthModule>>())
                .Tap(async (module) => await module.SendCommandAsync(CreateSuperAdminCommand.Create()))
                .Back()
                .SendSynchronouslyAndMapToResult(new GetRegistrationSummaryHttpRequest(E2EConstants.DefaultUserEmail))
                .Tap(async (response) => _registrationToken = await response.GetResponseContentAsync<string>())
                .Back()
                .SendAsyncAndMapToResult(() =>
                    new CompleteRegistrationHttpRequest(
                        E2EConstants.DefaultUserEmail,
                        _registrationToken))
                .Back()
                .SendSynchronouslyAndMapToResult(LoginAsSuperAdmin)
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>())
                .Back()
                .SendSynchronously(new CreateLibrarianHttpRequest(_token).WithReaderGuid(_userGuid))
                .SendSynchronouslyAndMapToResult(LoginAsUser)
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>())
                .Back()
                .ActAsync(() => Task.Run(() =>
                {
                    var authContext = this.Kernel.Get<AuthContext>();
                    var token = authContext.RefreshTokens.First(p => p.UserGuid == _userGuid);
                    _tokenGuid = token.TokenGuid;
                }))
                .SendAsync(() => new RevokeTokenHttpRequest(_tokenGuid, _token));
        }

        protected override void ConfigureLogger()
        {
            Logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
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

        private LoginUserRequest LoginAsUser()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.DefaultUserEmail)
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