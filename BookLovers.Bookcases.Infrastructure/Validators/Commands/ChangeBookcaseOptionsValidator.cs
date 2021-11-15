using BookLovers.Bookcases.Application.Commands.Bookcases;
using BookLovers.Bookcases.Domain.Settings;
using FluentValidation;

namespace BookLovers.Bookcases.Infrastructure.Validators.Commands
{
    internal class ChangeBookcaseOptionsValidator : AbstractValidator<ChangeBookcaseOptionsCommand>
    {
        public ChangeBookcaseOptionsValidator()
        {
            RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null");

            When(p => p.WriteModel != null, () =>
            {
                RuleFor(p => p.WriteModel.BookcaseGuid).NotEmpty()
                    .WithMessage("Invalid bookcase guid.");

                RuleFor(p => p.WriteModel.SelectedOptions).NotNull()
                    .WithMessage("Selected bookcase options cannot be null.");

                When(p => p.WriteModel.SelectedOptions != null, () =>
                    RuleForEach(p => p.WriteModel.SelectedOptions)
                        .Must(p => BookcaseOptionType.Has(p.OptionType))
                        .WithMessage("Invalid bookcase option type"));
            });
        }
    }
}