using BookLovers.Auth.Domain.Tokens.Services;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Tokens
{
    public class CreateRefreshTokenCommand : ICommand
    {
        public RefreshTokenProperties TokenProperties { get; }

        public CreateRefreshTokenCommand(RefreshTokenProperties tokenProperties)
        {
            this.TokenProperties = tokenProperties;
        }
    }
}