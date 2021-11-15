using System;
using BookLovers.Auth.Infrastructure.Services.Hashing;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Auth.Tests.UnitTests.HasherTests
{
    [TestFixture]
    public class Pbkdf2HasherTests
    {
        private IHasher _hasher;

        [Test]
        [TestCase("ABC")]
        [TestCase("Babcia123!")]
        [TestCase("xgzQwtz")]
        [TestCase("password")]
        public void GetSalt_WhenCalled_ShouldReturnSalt(string password)
        {
            var result = _hasher.GetSalt(password);

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }

        [Test]
        [TestCase("   ")]
        [TestCase("")]
        [TestCase(null)]
        public void GetSalt_PasswordIsEmpty_ShouldThrowArgumentException(string password)
        {
            Action act = () => _hasher.GetSalt(password);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        [TestCase("ABC")]
        [TestCase("Babcia123")]
        [TestCase("mamatata")]
        [TestCase("password")]
        public void GetHash_WhenCalled_ShouldReturnHash(string password)
        {
            var salt = _hasher.GetSalt(password);

            var hash = _hasher.GetHash(password, salt);

            hash.Should().NotBeNull();
            hash.Should().NotBeEmpty();
        }

        [Test]
        [TestCase("   ")]
        [TestCase("")]
        [TestCase(null)]
        public void GetHash_WhenInputIsEmpty_ShouldThrowArgumentException(string input)
        {
            Action act = () => _hasher.GetHash(input, string.Empty);

            act.Should().Throw<ArgumentException>();
        }

        [SetUp]
        public void SetUp()
        {
            _hasher = new Pbkdf2Hasher();
        }
    }
}