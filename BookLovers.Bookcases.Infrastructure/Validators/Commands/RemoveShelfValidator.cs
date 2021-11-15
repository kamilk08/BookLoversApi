using BookLovers.Bookcases.Application.Commands.Shelves;
using FluentValidation;

namespace BookLovers.Bookcases.Infrastructure.Validators.Commands
{
    internal class RemoveShelfValidator : AbstractValidator<RemoveShelfCommand>
    {
        public RemoveShelfValidator()
        {
            RuleFor(p => p.ShelfWriteModel).NotNull().WithMessage("Dto cannot be null");

            When(p => p.ShelfWriteModel != null, () =>
            {
                RuleFor(p => p.ShelfWriteModel.BookcaseGuid)
                    .NotEmpty().WithMessage("Invalid bookcase guid.");

                RuleFor(p => p.ShelfWriteModel.ShelfGuid)
                    .NotEmpty().WithMessage("Invalid shelf guid.");
            });
        }
    }
}