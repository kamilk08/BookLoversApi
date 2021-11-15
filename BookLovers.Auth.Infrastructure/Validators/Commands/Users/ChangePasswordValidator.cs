using BookLovers.Auth.Application.Commands.Users;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Users
{
    internal class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordValidator()
        {
            RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null");

            When(p => p.WriteModel != null, () =>
            {
                RuleFor(p => p.WriteModel.Password)
                    .NotEmpty().WithMessage("Password cannot be null or empty")
                    .MinimumLength(8).WithMessage("Password should contain at least 8 characters")
                    .Must(Base.Infrastructure.Validation.PasswordValidator.HasNumber)
                    .WithMessage("Password should contain at least one number")
                    .Must(Base.Infrastructure.Validation.PasswordValidator.HasUpperCase)
                    .WithMessage("Password should contain at least one uppercase letter");

                RuleFor(p => p.WriteModel.NewPassword).NotEmpty().WithMessage("New password cannot be empty")
                    .MinimumLength(8).WithMessage("New password should contain at least 8 characters")
                    .Must(Base.Infrastructure.Validation.PasswordValidator.HasNumber)
                    .WithMessage("New password should contain at least one number")
                    .Must(Base.Infrastructure.Validation.PasswordValidator.HasUpperCase)
                    .WithMessage("New password should contain at least one uppercase letter");
            });
        }
    }
}