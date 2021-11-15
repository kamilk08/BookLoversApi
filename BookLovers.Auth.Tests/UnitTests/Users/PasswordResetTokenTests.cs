using System;
using System.Threading.Tasks;
using AutoFixture;
using BookLovers.Auth.Domain.PasswordResets;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Auth.Tests.UnitTests.Users
{
    [TestFixture]
    public class PasswordResetTokenTests
    {
        private PasswordResetToken _passwordResetToken;
        private Fixture _fixture;
        private Guid _userGuid;
        private string _email;

        [Test]
        public async Task IsExpired_WhenInvokedAndTokenHasExpired_ThenShouldReturnFalse()
        {
            _passwordResetToken = new PasswordResetToken(_userGuid, _email, DateTime.UtcNow, _fixture.Create<string>());

            await Task.Delay(TimeSpan.FromSeconds(1));

            _passwordResetToken.IsExpired().Should().BeTrue();
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _userGuid = _fixture.Create<Guid>();
            _email = _fixture.Create<string>();
            _passwordResetToken = new PasswordResetToken(_userGuid, _email);
        }
    }
}