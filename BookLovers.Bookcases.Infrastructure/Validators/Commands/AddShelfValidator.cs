using BookLovers.Bookcases.Application.Commands.Shelves;
using FluentValidation;

namespace BookLovers.Bookcases.Infrastructure.Validators.Commands
{
    internal class AddShelfValidator : AbstractValidator<AddShelfCommand>
    {
        public AddShelfValidator() => When(p => p.WriteModel != null, () =>
        {
            RuleFor(p => p.WriteModel.ShelfGuid)
                .NotEmpty().WithMessage("Invalid shelf guid.");

            RuleFor(p => p.WriteModel.ShelfName)
                .NotEmpty().WithMessage("Shelf name cannot be empty")
                .Must(ValidShelfName)
                .When(p => !string.IsNullOrEmpty(p.WriteModel.ShelfName))
                .WithMessage(
                    "Invalid shelf name. Shelf name must contain at least 3 characters and cannot be empty or cannot contain only null spaces");

            RuleFor(p => p.WriteModel.BookcaseGuid).NotEmpty().WithMessage("Bookcase guid cannot be empty");
        });

        private bool ValidShelfName(string shelfName) =>
            shelfName.Length >= 3 && !string.IsNullOrWhiteSpace(shelfName.Trim());
    }
}