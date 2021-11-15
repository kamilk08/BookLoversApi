using BookLovers.Auth.Application.Commands.Tokens;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Tokens
{
    internal class CreateAccessTokenValidator : AbstractValidator<CreateAccessTokenCommand>
    {
        public CreateAccessTokenValidator()
        {
            RuleFor(p => p.Dto).NotNull().WithMessage("Dto cannot be null");
            RuleFor(p => p.TokenGuid).NotEmpty().WithMessage("Token guid cannot be empty");

            When(p => p.Dto != null, () =>
            {
                RuleFor(p => p.Dto.Claims)
                    .NotNull()
                    .WithMessage("Claims are invalid");

                RuleFor(p => p.Dto.Issuer)
                    .NotEmpty().WithMessage("Issuer cannot be empty");

                RuleFor(p => p.Dto.AudienceId)
                    .NotEmpty().WithMessage("AudienceId cannot be empty");

                RuleFor(p => p.Dto.SigningKey)
                    .NotEmpty().WithMessage("Signing key cannot be empty");
            });
        }
    }
}