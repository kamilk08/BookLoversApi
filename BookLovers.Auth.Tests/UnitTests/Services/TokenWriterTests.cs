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
    public class TokenWriterTests
    {
        private JwtTokenWriter _jwtTokenWriter;
        private ITokenDescriptor<SecurityTokenDescriptor> _tokenDescriptor;
        private Fixture _fixture;

        [Test]
        public void WriteToken_WhenCalled_ShouldReturnProtectedToken()
        {
            var tokenDescriptor = _tokenDescriptor.GetTokenDescriptor();

            var token = _jwtTokenWriter.WriteToken(tokenDescriptor);

            token.Should().NotBeEmpty();
        }

        [Test]
        public void WriteToken_WhenCalledAndTokenDescriptorIsNull_ShouldThrowArgumentNullException()
        {
            _tokenDescriptor = null;

            Action act = () => _jwtTokenWriter.WriteToken(_tokenDescriptor);

            act.Should().Throw<NullReferenceException>();
        }

        [SetUp]
        public void SetUp()
        {
            this._fixture = new Fixture();

            _jwtTokenWriter = new JwtTokenWriter();
            _tokenDescriptor = new JwtTokenDescriptorBuilder();
            _tokenDescriptor
                .AddAudience(_fixture.Create<string>())
                .AddClaims(new List<Claim>())
                .AddIssuer(_fixture.Create<string>())
                .AddSigningCredentials(_fixture.Create<string>())
                .AddAudience(_fixture.Create<string>())
                .AddIssuedAndExpiresDate(DateTimeOffset.UtcNow, DateTimeOffset.Now.AddMinutes(5));
        }
    }
}