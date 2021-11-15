using BookLovers.Bookcases.Application.Commands.Shelves;
using FluentValidation;

namespace BookLovers.Bookcases.Infrastructure.Validators.Commands
{
    internal class ChangeShelfNameValidator : AbstractValidator<ChangeShelfNameCommand>
    {
        public ChangeShelfNameValidator()
        {
            RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null.");
            When(p => p.WriteModel != null, () =>
            {
                RuleFor(p => p.WriteModel.BookcaseGuid)
                    .NotEmpty().WithMessage("Invalid bookcase guid.");

                RuleFor(p => p.WriteModel.ShelfGuid)
                    .NotEmpty().WithMessage("Invalid shelf guid.");

                RuleFor(p => p.WriteModel.ShelfName)
                    .Must(ValidShelfName).WithMessage(
                        "Invalid shelf name. Shelf name must contain at least 3 characters and cannot be empty or contain only null spaces");
            });
        }

        private bool ValidShelfName(string shelfName) =>
            shelfName.Length >= 3 && !string.IsNullOrWhiteSpace(shelfName.Trim());
    }
}