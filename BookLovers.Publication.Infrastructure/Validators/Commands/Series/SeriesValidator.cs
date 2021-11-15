using BookLovers.Publication.Application.WriteModels.Series;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Series
{
    internal class SeriesValidator : AbstractValidator<SeriesWriteModel>
    {
        public SeriesValidator()
        {
            this.RuleFor(p => p.SeriesGuid)
                .NotNull().WithMessage("Series guid cannot be null")
                .NotEmpty().WithMessage("Series guid cannot be empty");

            this.RuleFor(p => p.SeriesName)
                .NotNull().WithMessage("Series name cannot be null")
                .NotEmpty().WithMessage("Series name cannot be empty")
                .MinimumLength(3).WithMessage("Series should have at least 3 characters")
                .MaximumLength(byte.MaxValue)
                .WithMessage("Series name cannot have more then 255 characters");
        }
    }
}