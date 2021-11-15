using BookLovers.Auth.Application.WriteModels;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Users
{
    internal class AccountValidator : AbstractValidator<AccountWriteModel>
    {
        public AccountValidator()
        {
            RuleFor(p => p.AccountDetails)
                .NotNull()
                .WithMessage("Account details data transferable object cannot be null")
                .SetValidator(new AccountDetailsValidator());

            RuleFor(p => p.AccountSecurity)
                .NotNull()
                .WithMessage("Account security data transferable object cannot be null")
                .SetValidator(new AccountSecurityValidator());
        }
    }
}