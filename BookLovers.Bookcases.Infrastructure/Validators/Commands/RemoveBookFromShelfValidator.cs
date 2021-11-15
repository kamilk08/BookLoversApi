using BookLovers.Bookcases.Application.Commands.Bookcases;
using FluentValidation;

namespace BookLovers.Bookcases.Infrastructure.Validators.Commands
{
    internal class RemoveBookFromShelfValidator : AbstractValidator<RemoveFromShelfCommand>
    {
        public RemoveBookFromShelfValidator()
        {
            RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null.");

            When(p => p.WriteModel != null, () =>
            {
                RuleFor(p => p.WriteModel.BookGuid)
                    .NotEmpty().WithMessage("Invalid book guid.");

                RuleFor(p => p.WriteModel.BookcaseGuid)
                    .NotEmpty().WithMessage("Invalid bookcase guid.");

                RuleFor(p => p.WriteModel.ShelfGuid)
                    .NotEmpty().WithMessage("Invalid shelf guid.");
            });
        }
    }
}