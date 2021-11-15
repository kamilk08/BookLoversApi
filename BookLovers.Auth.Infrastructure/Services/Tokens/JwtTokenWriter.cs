using System.IdentityModel.Tokens.Jwt;
using BookLovers.Auth.Application.Contracts.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace BookLovers.Auth.Infrastructure.Services.Tokens
{
    public class JwtTokenWriter : ITokenWriter<SecurityTokenDescriptor>
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JwtTokenWriter()
        {
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string WriteToken(SecurityTokenDescriptor descriptor)
        {
            return _tokenHandler.WriteToken(_tokenHandler.CreateJwtSecurityToken(descriptor));
        }

        public string WriteToken(ITokenDescriptor<SecurityTokenDescriptor> descriptor)
        {
            return _tokenHandler.WriteToken(_tokenHandler.CreateJwtSecurityToken(descriptor.GetTokenDescriptor()));
        }
    }
}