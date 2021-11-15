using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Base.Infrastructure.Validation;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Users
{
    internal class ChangeEmailValidator : AbstractValidator<ChangeEmailCommand>
    {
        public ChangeEmailValidator()
        {
            RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null");

            When(p => p.WriteModel != null, () =>
            {
                RuleFor(user => user.WriteModel.Email)
                    .NotNull()
                    .WithMessage("User email cannot be null")
                    .NotEmpty()
                    .WithMessage("User email cannot be empty")
                    .Must(EmailValidator.IsValidEmail)
                    .WithMessage("Invalid email address")
                    .MaximumLength(byte.MaxValue)
                    .WithMessage("Maximum length of email is 255 characters")
                    .MinimumLength(3)
                    .WithMessage("Minimum length of email is 3 characters");

                RuleFor(p => p.WriteModel.NextEmail)
                    .NotNull().WithMessage("Previous email cannot be null").NotEmpty()
                    .WithMessage("Previous email cannot be empty").Must(EmailValidator.IsValidEmail)
                    .WithMessage("Previous email address is not valid").MaximumLength(254)
                    .WithMessage("Maximum length of email is 254 characters").MinimumLength(3)
                    .WithMessage("Minimum length of email is 3 characters");
            });
        }
    }
}