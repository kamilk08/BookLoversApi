using BookLovers.Auth.Application.Commands.Users;
using FluentValidation;

namespace BookLovers.Auth.Infrastructure.Validators.Commands.Users
{
    internal class BlockAccountValidator : AbstractValidator<BlockAccountCommand>
    {
        public BlockAccountValidator()
        {
            RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null");

            When(p => p.WriteModel != null, () =>
                RuleFor(p => p.WriteModel.BlockedReaderGuid)
                    .NotEmpty()
                    .WithMessage("Invalid user guid."));
        }
    }
}