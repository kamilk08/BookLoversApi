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

namespace BookLovers.Auth.Tests.UnitTests.FactoryTests
{
    [TestFixture]
    public class UserFactoryShouldCheckValidityOfUserName
    {
        private UserFactory _factory;
        private Mock<IEmailUniquenessChecker> _emailUniquenessCheckerMock;
        private Mock<IUserNameUniquenessChecker> _userNameUniquenessCheckerMock;
        private Fixture _fixture;
        private UserFactoryData _factoryData;

        [Test]
        public void Create_WhenCalledAndUserNameIsNotUnique_ShouldThrowBusinessRuleNotMeetException()
        {
            Action act = () => _factory.CreateUser();

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void Create_WhenCalledAndEmailIsNotUnique_ShouldThrowBusinessRuleNotMeetException()
        {
            _emailUniquenessCheckerMock.Setup(s => s.IsEmailUnique(_factoryData.AccountData.Email))
                .Returns(false);

            Action act = () => _factory.CreateUser();

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Email is not unique.");
        }

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();

            var hasherMock = new Mock<IHasher>();
            hasherMock.Setup(p => p.GetSalt(It.IsAny<string>())).Returns<string>(s => s);
            hasherMock.Setup(p => p.GetHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string t1, string t2) => t1 + t2);

            var userRepositoryMock = new Mock<IUserRepository>();

            var encryptionService = new HashingService(hasherMock.Object);

            _factoryData = UserFactoryData.Initialize()
                .WithBasics(_fixture.Create<Guid>(), _fixture.Create<string>())
                .WithAccount(_fixture.Create<string>(), _fixture.Create<string>());

            var roleProviderMock = new Mock<IRoleProvider>();
            this._emailUniquenessCheckerMock = new Mock<IEmailUniquenessChecker>();
            this._userNameUniquenessCheckerMock = new Mock<IUserNameUniquenessChecker>();

            roleProviderMock
                .Setup(p => p.GetRole(Role.Reader))
                .Returns<object>((p) => new UserRole(Role.Reader));

            this._emailUniquenessCheckerMock.Setup(p => p.IsEmailUnique(_factoryData.AccountData.Email))
                .Returns(true);

            this._userNameUniquenessCheckerMock.Setup(p => p.IsUserNameUnique(_factoryData.BasicData.Username))
                .Returns(false);

            var userFactorySetup = new UserFactorySetup(
                _userNameUniquenessCheckerMock.Object,
                _emailUniquenessCheckerMock.Object,
                encryptionService,
                roleProviderMock.Object);

            _factory = userFactorySetup.SetupFactory(_factoryData);
        }
    }
}