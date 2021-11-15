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

namespace BookLovers.Auth.Tests.EndToEndTests.ChangeEmail
{
    public class ChangeEmailTestAndUserHasInvalidToken : EndToEndTest
    {
        private Guid _userGuid;
        private JsonWebToken _token;
        private string _registrationToken;

        [Test]
        public async Task ChangeEmail_WhenCalledAndUserInvalidToken_ShouldReturnUnAuthorizedResponse()
        {
            await E2ETester
                .ArePrecedingRequestsSuccessful(flag => flag.Should().BeTrue())
                .SendAsyncAndMapToResult(() =>
                    new ChangeEmailHttpRequest(
                        "testEmail112@gmail.com",
                        E2EConstants.DefaultUserEmail,
                        E2EConstants.DefaultPassword,
                        JsonWebToken.EmptyToken()))
                .Tap(result => { result.IsUnAuthorized().Should().BeTrue(); });
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
                .SendAsync(RegisterUser())
                .SendSynchronouslyAndMapToResult(new GetRegistrationSummaryHttpRequest(E2EConstants.DefaultUserEmail))
                .Tap(async (response) => _registrationToken = await response.GetResponseContentAsync<string>())
                .Back()
                .SendSynchronously(
                    new CompleteRegistrationHttpRequest(
                        E2EConstants.DefaultUserEmail,
                        _registrationToken))
                .SendAsyncAndMapToResult(LoginAsUser)
                .Tap(async (response) => _token = await response.GetResponseContentAsync<JsonWebToken>());
        }

        protected override void ConfigureLogger()
        {
            Logger = new LoggerConfiguration().Enrich.WithExceptionDetails().MinimumLevel.Debug().WriteTo
                .Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
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