using BookLovers.Auth.Application.WriteModels;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Users
{
    internal class AccountSecurityValidator : AbstractValidator<AccountSecurityWriteModel>
    {
        public AccountSecurityValidator()
        {
            RuleFor(p => p.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Password cannot be null or empty")
                .MinimumLength(8).WithMessage("Password should contain at least 8 characters")
                .Must(Base.Infrastructure.Validation.PasswordValidator.HasNumber)
                .WithMessage("Password should contain atleast one number")
                .Must(Base.Infrastructure.Validation.PasswordValidator.HasUpperCase)
                .WithMessage("Password should contain at least one uppercase letter");
        }
    }
}