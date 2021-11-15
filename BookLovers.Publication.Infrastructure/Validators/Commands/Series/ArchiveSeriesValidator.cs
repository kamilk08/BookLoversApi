using BookLovers.Publication.Application.Commands.Series;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Series
{
    internal class ArchiveSeriesValidator : AbstractValidator<ArchiveSeriesCommand>
    {
        public ArchiveSeriesValidator()
        {
            this.RuleFor(p => p.SeriesGuid)
                .NotEmpty()
                .WithMessage("Series guid cannot be empty.");
        }
    }
}