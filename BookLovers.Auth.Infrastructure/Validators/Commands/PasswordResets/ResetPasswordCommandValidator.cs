using BookLovers.Auth.Application.Commands.PasswordResets;
using BookLovers.Base.Infrastructure.Validation;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.PasswordResets
{
    internal class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(p => p.WriteModel).NotNull().WithMessage("Invalid dto");
            When(p => p.WriteModel != null, () =>
            {
                RuleFor(p => p.WriteModel.Token).NotEmpty()
                    .WithMessage("Invalid token");

                RuleFor(p => p.WriteModel.Password).NotEmpty()
                    .WithMessage("New password cannot be empty")
                    .MinimumLength(8).WithMessage("New password should contain at least 8 characters")
                    .Must(PasswordValidator.HasNumber).WithMessage("New password should contain at least one number")
                    .Must(PasswordValidator.HasUpperCase)
                    .WithMessage("New password should contain at least one uppercase letter");
            });
        }
    }
}