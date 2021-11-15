using BookLovers.Auth.Application.Commands.Tokens;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Tokens
{
    internal class RevokeTokenValidator : AbstractValidator<RevokeTokenCommand>
    {
        public RevokeTokenValidator()
        {
            RuleFor(p => p.WriteModel)
                .NotNull().WithMessage("Dto cannot be null.");

            When(
                p => p.WriteModel != null,
                () => RuleFor(p => p.WriteModel.TokenGuid)
                        .NotEmpty().WithMessage("Invalid token guid."));
        }
    }
}