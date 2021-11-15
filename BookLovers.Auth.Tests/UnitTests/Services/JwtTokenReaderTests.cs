using System;
using System.Collections.Generic;
using System.Linq;
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
    public class JwtTokenReaderTests
    {
        private Fixture _fixture;
        private JwtTokenReader _jwtTokenReader;
        private ITokenDescriptor<SecurityTokenDescriptor> _tokenDescriptor;
        private ITokenWriter<SecurityTokenDescriptor> _tokenWriter;
        private string _protectedToken;
        private string _audience;
        private string _signingKey;
        private string _issuer;

        [Test]
        public void ReadToken_WhenCalled_ShouldReadProtectedToken()
        {
            var actualValue = _jwtTokenReader
                .AddProtectedToken(_protectedToken)
                .AddIssuer(_issuer)
                .AddAudience(_audience)
                .AddSigningKey(_signingKey)
                .ValidateIssuer()
                .ValidateAudience().ReadToken();

            actualValue.Should().NotBeNull();

            actualValue.Claims.ToList().Should().ContainSingle(p => p.Value == "testemail@gmail.com");
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _jwtTokenReader = new JwtTokenReader();

            _signingKey = _fixture.Create<string>();

            _audience = _fixture.Create<string>();

            _issuer = _fixture.Create<string>();

            _tokenWriter = new JwtTokenWriter();

            _tokenDescriptor = new JwtTokenDescriptorBuilder();

            _tokenDescriptor
                .AddAudience(_audience)
                .AddIssuer(_issuer)
                .AddSigningCredentials(_signingKey)
                .AddClaims(
                    new List<Claim>
                    {
                        new Claim("email", "testemail@gmail.com")
                    })
                .AddIssuedAndExpiresDate(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(30.0));

            _protectedToken = _tokenWriter.WriteToken(_tokenDescriptor);
        }
    }
}