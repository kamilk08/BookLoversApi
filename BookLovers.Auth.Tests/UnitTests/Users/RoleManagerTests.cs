using System;
using AutoFixture;
using BookLovers.Auth.Domain.Roles;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Domain.Users.Services.Factories;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Auth.Infrastructure.Services.Hashing;
using BookLovers.Base.Domain.Rules;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BookLovers.Auth.Tests.UnitTests.Users
{
    [TestFixture]
    public class RoleManagerTests
    {
        private Fixture _fixture;
        private User _user;
        private RoleManager _manager;
        private Mock<IRoleProvider> _roleProviderMock;
        private UserFactory _factory;
        private UserFactoryData _factoryData;

        [Test]
        public void Promote_WhenCalled_ShouldPromoteUserToSelectedRole()
        {
            RoleManager.Promote(_user, Role.Librarian);

            _user.Should().NotBeNull();
            var result = _user.IsInRole(Role.Librarian.Name);

            result.Should().BeTrue();
        }

        [Test]
        public void Promote_WhenCalledAndUserAccountIsBlocked_ShouldThrowBusinessRuleNotMeetException()
        {
            _user.BlockAccount();

            Action act = () => RoleManager.Promote(_user, Role.Librarian);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("User cannot be promoted to selected role.");
        }

        [Test]
        public void Promote_WhenCalledAndUserAccountHasBeenSuspendedInThePast_ShouldThrowBusinessRuleNotMeetException()
        {
            _user.BlockAccount();

            Action act = () => RoleManager.Promote(_user, Role.Librarian);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("User cannot be promoted to selected role.");
        }

        [Test]
        public void Degrade_WhenCalled_ShouldDegradeUser()
        {
            RoleManager.Promote(_user, Role.Librarian);

            RoleManager.Degrade(_user, Role.Librarian);

            var result = _user.IsInRole(Role.Librarian.Name);

            result.Should().BeFalse();
        }

        [Test]
        public void Degrade_WhenCalledAndUserDoesNotHaveAnyOtherRules_ShouldThrowBusinessRuleNotMeetException()
        {
            Action act = () => RoleManager.Degrade(_user, Role.Reader);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("User must have at least one role.");
        }

        [Test]
        public void Degrade_WhenCalledAndUserHasOnlyLibrarianRole_ShouldThrowBusinessRuleNotMeetException()
        {
            _roleProviderMock.Setup(p => p.GetRole(Role.Reader))
                .Returns<object>(p => new UserRole(Role.Librarian));

            var user = _factory.CreateUser();

            Action act = () => RoleManager.Degrade(user, Role.Librarian);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();
            this._manager = new RoleManager();

            _factoryData = UserFactoryData.Initialize()
                .WithBasics(_fixture.Create<Guid>(), _fixture.Create<string>())
                .WithAccount(_fixture.Create<string>(), _fixture.Create<string>());

            var hasher = new Mock<IHasher>();
            hasher.Setup(p => p.GetSalt(It.IsAny<string>())).Returns<string>(s => s);
            hasher.Setup(p => p.GetHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string t1, string t2) => t1 + t2);

            var userRepositoryMock = new Mock<IUserRepository>();
            var hashingService = new HashingService(hasher.Object);
            _roleProviderMock = new Mock<IRoleProvider>();
            var emailUniquenessChecker = new Mock<IEmailUniquenessChecker>();
            var userNameUniquenessCheckerMock = new Mock<IUserNameUniquenessChecker>();

            _roleProviderMock.Setup(p => p.GetRole(Role.Reader))
                .Returns<object>(p => new UserRole(Role.Reader));

            emailUniquenessChecker.Setup(p => p.IsEmailUnique(_factoryData.AccountData.Email))
                .Returns(true);

            userNameUniquenessCheckerMock.Setup(p => p.IsUserNameUnique(_factoryData.BasicData.Username))
                .Returns(true);

            var userFactorySetup = new UserFactorySetup(
                userNameUniquenessCheckerMock.Object,
                emailUniquenessChecker.Object,
                hashingService,
                _roleProviderMock.Object);

            _factory = userFactorySetup.SetupFactory(_factoryData);

            this._user = _factory.CreateUser();
        }
    }
}