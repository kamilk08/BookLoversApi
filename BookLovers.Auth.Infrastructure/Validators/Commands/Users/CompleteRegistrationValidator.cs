using BookLovers.Auth.Application.Commands.Registrations;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Users
{
    internal class CompleteRegistrationValidator : AbstractValidator<CompleteRegistrationCommand>
    {
        public CompleteRegistrationValidator()
        {
            RuleFor(p => p.Email).NotNull().NotEmpty().WithMessage("Invalid email.");

            RuleFor(p => p.Token).NotEmpty().WithMessage("Invalid token.");
        }
    }
}