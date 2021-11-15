using BookLovers.Auth.Application.Commands.Users;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Users
{
    internal class SignUpValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpValidator()
        {
            RuleFor(p => p.WriteModel).NotNull().WithMessage("Data transferable object cannot be null");
            When(p => p.WriteModel != null, () =>
            {
                RuleFor(p => p.WriteModel.UserGuid)
                    .NotNull().WithMessage("Reader guid cannot be null")
                    .NotEmpty()
                    .WithMessage("Reader guid cannot be empty");

                RuleFor(p => p.WriteModel.BookcaseGuid)
                    .NotNull()
                    .WithMessage("Bookcase guid cannot be null")
                    .NotEmpty()
                    .WithMessage("Bookcase guid cannot be empty");

                RuleFor(p => p.WriteModel.Account)
                    .NotNull()
                    .WithMessage("Account dto cannot be empty");

                When(p => p.WriteModel.Account != null, () =>
                {
                    RuleFor(p => p.WriteModel.Account.AccountDetails)
                        .SetValidator(new AccountDetailsValidator())
                        .WithMessage("Account details cannot be null");

                    RuleFor(p => p.WriteModel.Account.AccountSecurity)
                        .SetValidator(new AccountSecurityValidator())
                        .WithMessage("Account security cannot be null");
                });
            });
        }
    }
}