using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure.Validation;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Users
{
    internal class AccountDetailsValidator : AbstractValidator<AccountDetailsWriteModel>
    {
        public AccountDetailsValidator()
        {
            RuleFor(p => p.Email)
                .Must(EmailValidator.IsValidEmail)
                .WithMessage("Email is not valid");

            RuleFor(p => p.UserName)
                .NotNull()
                .WithMessage("Username cannot be null")
                .NotEmpty()
                .WithMessage("Username cannot be empty")
                .MinimumLength(3)
                .WithMessage("Username length cannot be less then 3 characters").MaximumLength(100)
                .WithMessage("Username length cannot be higher then 100 characters").Must(StringValidator.NoWhiteSpace)
                .WithMessage("Username cannot have white spaces");
        }
    }
}