using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Tokens;
using BookLovers.Auth.Application.Contracts.Tokens;
using BookLovers.Auth.Domain.Tokens.Services;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;

namespace BookLovers.Auth.Application.CommandHandlers.Tokens
{
    internal class CreateRefreshTokenHandler : ICommandHandler<CreateRefreshTokenCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenManager _tokenManager;
        private readonly ITokenDescriptor<SecurityTokenDescriptor> _tokenDescriptor;
        private readonly ITokenWriter<SecurityTokenDescriptor> _tokenWriter;
        private readonly RefreshTokenFactory _tokenFactory;

        public CreateRefreshTokenHandler(
            IUserRepository userRepository,
            ITokenManager tokenManager,
            ITokenDescriptor<SecurityTokenDescriptor> tokenDescriptor,
            ITokenWriter<SecurityTokenDescriptor> tokenWriter,
            RefreshTokenFactory tokenFactory)
        {
            this._userRepository = userRepository;
            this._tokenManager = tokenManager;
            this._tokenDescriptor = tokenDescriptor;
            this._tokenWriter = tokenWriter;
            this._tokenFactory = tokenFactory;
        }

        public async Task HandleAsync(CreateRefreshTokenCommand command)
        {
            var tokenDescriptor = this._tokenDescriptor
                .AddIssuer(command.TokenProperties.Issuer)
                .AddClaims(new List<Claim>() { new Claim("jti", command.TokenProperties.TokenGuid.ToString()) })
                .AddAudience(command.TokenProperties.AudienceGuid.ToString())
                .AddSigningCredentials(command.TokenProperties.SigningKey)
                .AddIssuedAndExpiresDate(
                    command.TokenProperties.IssuedAt.GetValueOrDefault(),
                    command.TokenProperties.ExpiresAt.GetValueOrDefault());

            var token = this._tokenWriter.WriteToken(tokenDescriptor);

            await this._tokenFactory.Create(token, command.TokenProperties);

            await this._tokenManager.AddTokenAsync(command.TokenProperties.TokenGuid, token);
        }
    }
}