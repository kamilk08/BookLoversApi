using BookLovers.Auth.Application.Commands.Users;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands
{
    internal class CreateSuperAdminValidator : AbstractValidator<CreateSuperAdminCommand>
    {
        public CreateSuperAdminValidator()
        {
            RuleFor(p => p.SignUpWriteModel).NotNull();
            When(p => p.SignUpWriteModel != null, () =>
            {
                RuleFor(p => p.SignUpWriteModel.Account)
                    .NotNull().WithMessage("Invalid user account");

                RuleFor(p => p.SignUpWriteModel.BookcaseGuid)
                    .NotEmpty().WithMessage("Invalid bookcase guid");

                RuleFor(p => p.SignUpWriteModel.ProfileGuid)
                    .NotEmpty().WithMessage("Invalid profile guid");

                RuleFor(p => p.SignUpWriteModel.UserGuid)
                    .NotEmpty().WithMessage("Invalid user guid");
            });
        }
    }
}