using System;
using System.Threading.Tasks;
using AutoFixture;
using BookLovers.Auth.Domain.Roles;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Domain.Users.Services.Factories;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Auth.Infrastructure.Services.Authorization;
using BookLovers.Auth.Infrastructure.Services.Hashing;
using BookLovers.Base.Infrastructure.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BookLovers.Auth.Tests.UnitTests.Services
{
    [TestFixture]
    public class AuthorizationServiceTests
    {
        private IAuthorizeService _authorizeService;
        private Mock<IUserRepository> _userRepositoryMock;
        private User _user;
        private Fixture _fixture;

        [Test]
        public async Task AuthorizeAsync_WhenCalled_ShouldReturnTrue()
        {
            var result = await _authorizeService.AuthorizeAsync(_user.Guid, Role.Reader.Name);

            _userRepositoryMock.Verify(p => p.GetAsync(_user.Guid), Times.Once);

            result.Should().BeTrue();
        }

        [Test]
        public async Task AuthorizeAsync_WhenCalledAndUserAccountIsBlocker_ShouldReturnFalse()
        {
            _user.BlockAccount();

            var result = await _authorizeService.AuthorizeAsync(_user.Guid, Role.Reader.Name);

            result.Should().BeFalse();

            _userRepositoryMock.Verify(p => p.GetAsync(_user.Guid), Times.Once);
        }

        [Test]
        public async Task AuthorizeAsync_WhenCalledWithUserThatSupposedToBeLibrarianButHesNot_ShouldReturnFalse()
        {
            var result = await _authorizeService.AuthorizeAsync(_user.Guid, Role.Librarian.Name);

            _userRepositoryMock.Verify(p => p.GetAsync(_user.Guid), Times.Once);

            result.Should().BeFalse();
        }

        [Test]
        public async Task AuthorizeAsync_WhenCalledAndUserIsALibrarian_ShouldReturnTrue()
        {
            RoleManager.Promote(_user, Role.Librarian);

            var result = await _authorizeService.AuthorizeAsync(_user.Guid, Role.Librarian.Name);

            result.Should().BeTrue();

            _userRepositoryMock.Verify(p => p.GetAsync(_user.Guid), Times.Once);
        }

        [Test]
        public async Task AuthorizeAsync_WhenCalledAndUserIsALibrarianButHesAuthorizingAsReader_ShouldReturnTrue()
        {
            RoleManager.Promote(_user, Role.Librarian);

            var result = await _authorizeService.AuthorizeAsync(_user.Guid, Role.Reader.Name);

            result.Should().BeTrue();

            _userRepositoryMock.Verify(p => p.GetAsync(_user.Guid), Times.Once);
        }

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();

            var factoryData = UserFactoryData.Initialize()
                .WithBasics(_fixture.Create<Guid>(), _fixture.Create<string>())
                .WithAccount(_fixture.Create<string>(), _fixture.Create<string>());

            var hasherMock = new Mock<IHasher>();
            hasherMock.Setup(p => p.GetSalt(It.IsAny<string>())).Returns<string>(s => s);
            hasherMock.Setup(p => p.GetHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string t1, string t2) => t1 + t2);

            var roleProviderMock = new Mock<IRoleProvider>();

            roleProviderMock
                .Setup(p => p.GetRole(Role.Reader))
                .Returns<object>((p) => new UserRole(Role.Reader));

            var emailUniquenessCheckerMock = new Mock<IEmailUniquenessChecker>();

            emailUniquenessCheckerMock
                .Setup(p => p.IsEmailUnique(factoryData.AccountData.Email))
                .Returns<object>((p) => true);

            var userNameUniquenessCheckerMock = new Mock<IUserNameUniquenessChecker>();

            userNameUniquenessCheckerMock
                .Setup(p => p.IsUserNameUnique(factoryData.BasicData.Username))
                .Returns(true);

            var hashingService = new HashingService(hasherMock.Object);

            var userFactorySetup = new UserFactorySetup(
                userNameUniquenessCheckerMock.Object,
                emailUniquenessCheckerMock.Object,
                hashingService,
                roleProviderMock.Object);

            var factory = userFactorySetup.SetupFactory(factoryData);

            _user = factory.CreateUser();

            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepositoryMock.Setup(p => p.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_user);

            _authorizeService = new AuthorizationService(_userRepositoryMock.Object);
        }
    }
}