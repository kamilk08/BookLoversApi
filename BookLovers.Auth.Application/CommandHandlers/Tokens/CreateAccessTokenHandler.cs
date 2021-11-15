using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Tokens;
using BookLovers.Auth.Application.Contracts.Tokens;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;

namespace BookLovers.Auth.Application.CommandHandlers.Tokens
{
    internal class CreateAccessTokenHandler : ICommandHandler<CreateAccessTokenCommand>
    {
        private readonly ITokenDescriptor<SecurityTokenDescriptor> _tokenDescriptor;
        private readonly ITokenWriter<SecurityTokenDescriptor> _tokenWriter;
        private readonly ITokenManager _manager;

        public CreateAccessTokenHandler(
            ITokenDescriptor<SecurityTokenDescriptor> tokenDescriptor,
            ITokenWriter<SecurityTokenDescriptor> tokenWriter,
            ITokenManager manager)
        {
            this._tokenDescriptor = tokenDescriptor;
            this._tokenWriter = tokenWriter;
            this._manager = manager;
        }

        public async Task HandleAsync(CreateAccessTokenCommand command)
        {
            var tokenDescriptor = this._tokenDescriptor
                .AddIssuer(command.Dto.Issuer)
                .AddAudience(command.Dto.AudienceId)
                .AddClaims(command.Dto.Claims)
                .AddIssuedAndExpiresDate(
                    command.Dto.IssuedAt.GetValueOrDefault(),
                    command.Dto.ExpiresAt.GetValueOrDefault())
                .AddSigningCredentials(command.Dto.SigningKey);

            var token = this._tokenWriter.WriteToken(tokenDescriptor);

            await this._manager.AddTokenAsync(command.TokenGuid, token);
        }
    }
}