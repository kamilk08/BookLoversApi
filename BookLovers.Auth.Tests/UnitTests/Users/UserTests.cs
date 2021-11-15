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
    public class UserAggregateTests
    {
        private Fixture _fixture;
        private IHashingService _hashingService;
        private Mock<IEmailUniquenessChecker> _emailUniquenessChecker;
        private User _user;

        [Test]
        public void ChangePassword_WhenCalled_ShouldChangePassword()
        {
            var newPassword = _fixture.Create<string>();
            var prevHash = _user.Account.AccountSecurity.Hash;
            var prevSalt = _user.Account.AccountSecurity.Salt;

            _user.ChangePassword(newPassword, _hashingService);

            _user.Account.AccountSecurity.Hash.Should().NotBe(prevHash);
            _user.Account.AccountSecurity.Salt.Should().NotBe(prevSalt);
            _user.Account.AccountSecurity.Hash.Should().NotBeNull();
            _user.Account.AccountSecurity.Salt.Should().NotBeNull();
            _user.Account.AccountSecurity.Salt.Should().Be(newPassword);
            _user.Account.AccountSecurity.Hash.Should().Be(newPassword + newPassword);
        }

        [Test]
        public void ChangePassword_WhenCalledAndAccountHasBeenBlocked_ShouldThrowBusinessRuleNotMeetException()
        {
            _user.BlockAccount();

            var newPassword = _fixture.Create<string>();

            Action act = () => _user.ChangePassword(newPassword, _hashingService);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Account cannot be blocked.");
        }

        [Test]
        public void ChangeEmail_WhenCalled_ShouldChangeUserEmail()
        {
            var newEmail = _fixture.Create<string>();
            _emailUniquenessChecker.Setup(p => p.IsEmailUnique(newEmail)).Returns(true);

            _user.ChangeEmail(newEmail, _emailUniquenessChecker.Object);

            _user.Account.Email.Value.Should().Be(newEmail);
        }

        [Test]
        public void ChangeEmail_WhenCalledAndUserAccountIsBlocked_ShouldThrowBussinesRuleNotMeetException()
        {
            _user.BlockAccount();

            var newEmail = _fixture.Create<string>();

            Action act = () => _user.ChangeEmail(newEmail, _emailUniquenessChecker.Object);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Account cannot be blocked.");
        }

        [Test]
        public void ChangeEmail_WhenCalledAndGivenEmailIsNotUnique_ShouldThrowBusinessRuleNotMeetException()
        {
            var newEmail = _fixture.Create<string>();
            _emailUniquenessChecker.Setup(p => p.IsEmailUnique(newEmail)).Returns(false);

            Action act = () => _user.ChangeEmail(newEmail, _emailUniquenessChecker.Object);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Email is not unique.");
        }

        [Test]
        public void ConfirmEmail_WhenCalled_ShouldConfirmEmail()
        {
            _user.ConfirmAccount(DateTime.UtcNow);

            _user.Account.Email.Should().NotBeNull();
            _user.Account.Email.Value.Should().NotBeNull();
        }

        [Test]
        public void GetRole_WhenCalled_ShouldReturnSelectedRole()
        {
            var role = _user.GetRole(Role.Reader.Name);

            role.Should().NotBeNull();
            role.Role.Should().Be(Role.Reader);
        }

        [Test]
        public void GetRole_WhenCalledAndUserDoesNotHaveSelectedRole_ShouldReturnNull()
        {
            var role = _user.GetRole(Role.Librarian.Name);

            role.Should().BeNull();
        }

        [Test]
        public void IsInRole_WhenCalled_ShouldReturnTrue()
        {
            var result = _user.IsInRole(Role.Reader.Name);

            result.Should().BeTrue();
        }

        [Test]
        public void IsInRole_WhenCalled_ShouldReturnFalse()
        {
            var result = _user.IsInRole(Role.Librarian.Name);

            result.Should().BeFalse();
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

            _hashingService = new HashingService(hasherMock.Object);
            var roleProviderMock = new Mock<IRoleProvider>();
            _emailUniquenessChecker = new Mock<IEmailUniquenessChecker>();
            var userNameUniquenessCheckerMock = new Mock<IUserNameUniquenessChecker>();

            roleProviderMock.Setup(p => p.GetRole(Role.Reader))
                .Returns<object>(p => new UserRole(Role.Reader));

            _emailUniquenessChecker.Setup(p => p.IsEmailUnique(factoryData.AccountData.Email))
                .Returns(true);

            userNameUniquenessCheckerMock.Setup(p => p.IsUserNameUnique(factoryData.BasicData.Username))
                .Returns(true);

            var userFactorySetup = new UserFactorySetup(
                userNameUniquenessCheckerMock.Object,
                _emailUniquenessChecker.Object,
                _hashingService,
                roleProviderMock.Object);

            var factory = userFactorySetup.SetupFactory(factoryData);

            this._user = factory.CreateUser();
        }
    }
}