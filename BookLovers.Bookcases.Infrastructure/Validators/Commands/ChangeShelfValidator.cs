using BookLovers.Bookcases.Application.Commands.Shelves;
using FluentValidation;

namespace BookLovers.Bookcases.Infrastructure.Validators.Commands
{
    internal class ChangeShelfValidator : AbstractValidator<ChangeShelfCommand>
    {
        public ChangeShelfValidator()
        {
            RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null");

            When(p => p.WriteModel != null, () =>
            {
                RuleFor(p => p.WriteModel.BookGuid)
                    .NotEmpty().WithMessage("Invalid book guid.");

                RuleFor(p => p.WriteModel.BookcaseGuid)
                    .NotEmpty().WithMessage("Invalid bookcase guid.");

                RuleFor(p => p.WriteModel.NewShelfGuid)
                    .NotEmpty().WithMessage("Invalid new shelf guid.");

                RuleFor(p => p.WriteModel.OldShelfGuid)
                    .NotEmpty().WithMessage("Invalid old shelf guid.");
            });
        }
    }
}