using BookLovers.Auth.Application.Commands.Tokens;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Tokens
{
    internal class RefreshTokenValidator : AbstractValidator<CreateRefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(p => p.TokenProperties)
                .NotNull().WithMessage("Invalid token properties");
        }
    }
}