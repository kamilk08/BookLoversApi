using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace BookLovers.Auth.Infrastructure.Services.Tokens
{
    public class JwtTokenReader
    {
        private readonly JwtSecurityTokenHandler _handler;
        private readonly TokenValidationParameters _validationParameters;
        private SecurityToken _securityToken;
        private string _token;

        public DateTime IssuedAtUtc =>
            this._securityToken?.ValidFrom.ToUniversalTime() ?? default(DateTime);

        public DateTime ExpiresUtc => _securityToken?.ValidTo.ToUniversalTime() ?? default(DateTime);

        public JwtTokenReader()
        {
            _handler = new JwtSecurityTokenHandler();
            _validationParameters = new TokenValidationParameters();
        }

        public JwtTokenReader AddProtectedToken(string protectedToken)
        {
            _token = protectedToken;

            return this;
        }

        public JwtTokenReader AddIssuer(string issuer)
        {
            _validationParameters.ValidIssuer = issuer;

            return this;
        }

        public JwtTokenReader AddAudience(string validAudience)
        {
            _validationParameters.ValidAudience = validAudience;

            return this;
        }

        public JwtTokenReader AddAudiences(IEnumerable<string> keys)
        {
            _validationParameters.ValidAudiences = keys.ToList();

            return this;
        }

        public JwtTokenReader AddSigningKey(string secretKey)
        {
            _validationParameters.IssuerSigningKey = SymmetricSecurityKeyFactory.CreateSingleKey(secretKey);

            return this;
        }

        public JwtTokenReader ValidateAudience()
        {
            _validationParameters.ValidateAudience = true;

            return this;
        }

        public JwtTokenReader ValidateIssuer()
        {
            _validationParameters.ValidateIssuer = true;
            _validationParameters.ValidateIssuerSigningKey = true;

            return this;
        }

        public ClaimsPrincipal ReadToken()
        {
            ClaimsPrincipal claimsPrincipal;

            try
            {
                claimsPrincipal = _handler.ValidateToken(_token, _validationParameters, out _securityToken);
            }
            catch (Exception ex)
            {
                return null;
            }

            return claimsPrincipal;
        }
    }
}