using System;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.HttpRequests.Auth;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;
using Serilog;
using Serilog.Exceptions;
using SimpleE2ETesterLibrary.Extensions.Responses;
using SimpleE2ETesterLibrary.Extensions.Tester;

namespace BookLovers.Auth.Tests.EndToEndTests.ChangePassword
{
    public class ChangePasswordTest : EndToEndTest
    {
        private string _registrationToken;
        private JsonWebToken _token;
        private Guid _userGuid;

        [Test]
        public async Task ChangePassword_WhenCalled_ShouldChangeUserPasswordAndReturnOkStatusCode()
        {
            await this.E2ETester
                .ArePrecedingRequestsSuccessful((flag) => flag.Should().BeTrue())
                .SendAsyncAndMapToResult(ChangePassword)
                .Tap((result) => result.IsOk().Should().BeTrue());
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

            _userGuid = Fixture.Create<Guid>();

            var httpContextAccessor = new FakeHttpContextAccessor(_userGuid, true);

            var mock = new Mock<IAppManager>();

            mock.Setup(s => s.GetConfigValue(JwtSettings.Issuer))
                .Returns("http://localhost:64892/");

            mock.Setup(s => s.GetConfigValue(JwtSettings.JsonWebTokenKey))
                .Returns("mySuperSecretKey");

            var authConnectionString = Environment.GetEnvironmentVariable(AuthContext.ConnectionStringKey);
            if (authConnectionString.IsEmpty())
                authConnectionString = E2EConstants.AuthConnectionString;
            
            mock.Setup(s => s.GetConfigValue(AuthContext.ConnectionStringKey))
                .Returns(authConnectionString);

            kernel.Bind<IHttpContextAccessor>().ToConstant(httpContextAccessor);
            kernel.Bind<IAppManager>().ToConstant(mock.Object);
            kernel.Bind<ITokenManager>().To<TokenManager>();
        }

        protected override void InitializeModules(
            IHttpContextAccessor contextAccessor,
            IAppManager manager)
        {
            AuthModuleStartup.Initialize(contextAccessor, manager, Logger, PersistenceSettings.Default());
        }

        protected override async Task SendPreRequestsAsync()
        {
            await this.E2ETester
                .SendAsync(RegisterUser)
                .SendSynchronouslyAndMapToResult(new GetRegistrationSummaryHttpRequest(E2EConstants.DefaultUserEmail))
                .Tap(async (response) => _registrationToken = await response.GetResponseContentAsync<string>())
                .Back()
                .SendSynchronously(
                    new CompleteRegistrationHttpRequest(E2EConstants.DefaultUserEmail, _registrationToken))
                .SendAsyncAndMapToResult(LoginUser)
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>());
        }

        protected override void ConfigureLogger()
        {
            Logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails().MinimumLevel.Debug()
                .WriteTo
                .Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        private LoginUserRequest LoginUser()
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

        private ChangePasswordHttpRequest ChangePassword()
        {
            return new ChangePasswordHttpRequest(
                E2EConstants.DefaultPassword,
                "Babcia123!123",
                E2EConstants.DefaultUserEmail,
                _token);
        }

        private LoginUserRequest LoginWithNewPassword()
        {
            return new LoginUserRequest()
                .WithUserName(E2EConstants.DefaultUserNickName)
                .WithPassword("Babcia123!123")
                .WithClientId(E2EConstants.AudienceGuid)
                .WithClientSecret(E2EConstants.ClientSecret);
        }
    }
}