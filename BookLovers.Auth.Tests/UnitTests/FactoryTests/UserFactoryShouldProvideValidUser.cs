using System;
using AutoFixture;
using BookLovers.Auth.Domain.Roles;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Domain.Users.Services.Factories;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Auth.Infrastructure.Services.Hashing;
using BookLovers.Base.Domain.Aggregates;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BookLovers.Auth.Tests.UnitTests.FactoryTests
{
    [TestFixture]
    public class UserFactoryShouldProvideValidUser
    {
        private UserFactory _factory;
        private Mock<IEmailUniquenessChecker> _emailUniquenessCheckerMock;
        private Mock<IUserNameUniquenessChecker> _userNameUniquenessCheckerMock;
        private Mock<IRoleProvider> _roleProviderMock;
        private Fixture _fixture;

        private UserFactoryData _factoryData;

        [Test]
        public void Create_WhenCalled_ShouldReturnValidUser()
        {
            var user = this._factory.CreateUser();

            user.Should().NotBeNull();
            user.UserName.Value.Should().BeEquivalentTo(_factoryData.BasicData.Username);
            user.Account.Email.Value.Should().BeEquivalentTo(_factoryData.AccountData.Email);
            user.Guid.Should().Be(_factoryData.BasicData.UserGuid);
            user.Account.AccountSecurity.IsBlocked.Should().BeFalse();
            user.Account.AccountSecurity.Hash.Should().NotBeNull();
            user.Account.AccountSecurity.Salt.Should().NotBeNull();
            user.Roles.Should().ContainSingle(p => p.GetType() == typeof(UserRole));
            user.Status.Should().Be(AggregateStatus.Active.Value);
            user.Account.AccountDetails.AccountCreateDate.Should().Be(_factoryData.AccountData.AccountCreateDate);
            user.Account.AccountDetails.HasBeenBlockedPreviously.Should().BeFalse();
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

            this._roleProviderMock = new Mock<IRoleProvider>();
            this._emailUniquenessCheckerMock = new Mock<IEmailUniquenessChecker>();
            this._userNameUniquenessCheckerMock = new Mock<IUserNameUniquenessChecker>();

            this._roleProviderMock.Setup(p => p.GetRole(Role.Reader))
                .Returns<object>(p => new UserRole(Role.Reader));

            this._emailUniquenessCheckerMock.Setup(p => p.IsEmailUnique(_factoryData.AccountData.Email))
                .Returns(true);

            this._userNameUniquenessCheckerMock.Setup(p => p.IsUserNameUnique(_factoryData.BasicData.Username))
                .Returns(true);

            var userFactorySetup = new UserFactorySetup(
                _userNameUniquenessCheckerMock.Object,
                _emailUniquenessCheckerMock.Object,
                encryptionService,
                _roleProviderMock.Object);

            _factory = userFactorySetup.SetupFactory(_factoryData);
        }
    }
}