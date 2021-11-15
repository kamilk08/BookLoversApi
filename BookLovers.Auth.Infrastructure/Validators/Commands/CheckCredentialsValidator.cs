using BookLovers.Auth.Application.Commands.Users;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands
{
    internal class CheckCredentialsValidator : AbstractValidator<CheckCredentialsCommand>
    {
        public CheckCredentialsValidator()
        {
            RuleFor(p => p.Password).NotEmpty().WithMessage("Password cannot be empty");

            RuleFor(p => p.UserName).NotEmpty().WithMessage("Username cannot be empty");
        }
    }
}