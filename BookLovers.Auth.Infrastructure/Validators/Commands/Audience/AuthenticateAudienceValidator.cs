using BookLovers.Auth.Application.Commands.Audiences;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Audience
{
    internal class AuthenticateAudienceValidator : AbstractValidator<AuthenticateAudienceCommand>
    {
        public AuthenticateAudienceValidator()
        {
            RuleFor(p => p.AudienceId).NotEmpty().NotNull();

            RuleFor(p => p.SecretKey).NotEmpty().NotEmpty();
        }
    }
}