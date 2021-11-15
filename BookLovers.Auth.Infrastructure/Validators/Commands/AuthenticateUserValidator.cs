using BookLovers.Auth.Application.Commands.Users;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands
{
    internal class AuthenticateUserValidator : AbstractValidator<AuthenticateUserCommand>
    {
        public AuthenticateUserValidator()
        {
            RuleFor(p => p.Password).NotEmpty().WithMessage("Password cannot be empty");

            RuleFor(p => p.Email).NotEmpty().WithMessage("Email cannot be empty");
        }
    }
}