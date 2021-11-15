using System.Collections.Generic;
using AutoFixture;
using BookLovers.Auth.Infrastructure.Services.Authentication;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Auth.Tests.UnitTests.Services
{
    [TestFixture]
    public class ClaimsBuilderTests
    {
        private Fixture _fixture;
        private ClaimsBuilder _claimsBuilder;

        [Test]
        public void GetClaimsIdentity_WhenCalled_ShouldCreateClaims()
        {
            var claimsIdentity = _claimsBuilder.GetClaimsIdentity();
            claimsIdentity.Should().NotBeNull();
            claimsIdentity.AuthenticationType.Should().Be("Bearer");
            claimsIdentity.Claims.Should().NotBeEmpty();
            claimsIdentity.Claims.Should().ContainSingle(p => p.Type == "jti");
            claimsIdentity.Claims.Should().ContainSingle(p => p.Type == "sub");
            claimsIdentity.Claims.Should().ContainSingle(p => p.Type == "email");
            claimsIdentity.Claims.Should().ContainSingle(p => p.Type == "iat");
            claimsIdentity.Claims.Should().Contain(p => p.Type == "roles");
        }

        [Test]
        public void GetClaimsIdentity_WhenCalledWithCustomClaims_ShouldCreateIdentityWithCustomClaims()
        {
            var claimType = _fixture.Create<string>();
            var claimValue = _fixture.Create<string>();
            _claimsBuilder.AddCustomClaim(claimType, claimValue);
            _claimsBuilder.GetClaimsIdentity().Claims.Should().ContainSingle(p => p.Type == claimType).And
                .ContainSingle(p => p.Value == claimValue);
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _claimsBuilder = new ClaimsBuilder();
            _claimsBuilder.AddEmail(_fixture.Create<string>()).AddRoles(CreateRoles())
                .AddSubject(_fixture.Create<string>()).AddIssuedDate().AddTokenIdentifier();
        }

        private IEnumerable<string> CreateRoles()
        {
            for (var i = 0; i < 2; ++i)
                yield return _fixture.Create<string>();
        }
    }
}