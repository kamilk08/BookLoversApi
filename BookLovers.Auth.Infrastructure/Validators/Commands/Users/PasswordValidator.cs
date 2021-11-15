using BookLovers.Auth.Infrastructure.Dtos.Users;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Users
{
    internal class PasswordValidator : AbstractValidator<PasswordDto>
    {
        public PasswordValidator()
        {
            RuleFor(p => p.Password)
                .NotNull().WithMessage("Password cannot be null")
                .NotEmpty().WithMessage("Password cannot be empty")
                .Must(Base.Infrastructure.Validation.PasswordValidator.HasUpperCase)
                .WithMessage("Password should contain at least one uppercase letter")
                .Must(Base.Infrastructure.Validation.PasswordValidator.HasNumber)
                .WithMessage("Password should contain at least one number");
        }
    }
}