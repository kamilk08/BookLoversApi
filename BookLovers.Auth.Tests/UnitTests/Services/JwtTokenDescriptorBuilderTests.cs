using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoFixture;
using BookLovers.Auth.Application.Contracts.Tokens;
using BookLovers.Auth.Infrastructure.Services.Tokens;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;

namespace BookLovers.Auth.Tests.UnitTests.Services
{
    [TestFixture]
    public class JwtTokenDescriptorBuilderTests
    {
        private Fixture _fixture;
        private ITokenDescriptor<SecurityTokenDescriptor> _tokenDescriptor;

        [Test]
        public void GetTokenDescriptor_WhenCalled_ShouldReturnTokenDescriptor()
        {
            var tokenDescriptor = _tokenDescriptor.GetTokenDescriptor();
            tokenDescriptor.Audience.Should().NotBeEmpty();
            tokenDescriptor.Expires.Should().NotBeNull();
            tokenDescriptor.Issuer.Should().NotBeEmpty();
            tokenDescriptor.Subject.Should().NotBeNull();
            tokenDescriptor.SigningCredentials.Should().NotBeNull();
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _tokenDescriptor = new JwtTokenDescriptorBuilder();
            _tokenDescriptor
                .AddAudience(_fixture.Create<string>())
                .AddClaims(new List<Claim>())
                .AddIssuer(_fixture.Create<string>())
                .AddSigningCredentials(_fixture.Create<string>())
                .AddIssuedAndExpiresDate(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(5.0));
        }
    }
}