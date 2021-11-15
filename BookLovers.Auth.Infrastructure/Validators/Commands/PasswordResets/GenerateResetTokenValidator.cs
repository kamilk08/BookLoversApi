using BookLovers.Auth.Application.Commands.PasswordResets;
using BookLovers.Base.Infrastructure.Validation;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.PasswordResets
{
    internal class GenerateResetTokenValidator : AbstractValidator<GenerateResetTokenPasswordCommand>
    {
        public GenerateResetTokenValidator()
        {
            RuleFor(p => p.WriteModel).NotNull().WithMessage("Invalid dto");

            When(p => p.WriteModel != null, () =>
                RuleFor(p => p.WriteModel.Email)
                    .NotEmpty()
                    .WithMessage("Email cannot be empty")
                    .NotNull()
                    .WithMessage("Email cannot be null")
                    .Must(EmailValidator.IsValidEmail).WithMessage("Email is not valid."));
        }
    }
}