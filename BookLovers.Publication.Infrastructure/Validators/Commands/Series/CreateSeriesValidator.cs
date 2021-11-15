using BookLovers.Publication.Application.Commands.Series;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Series
{
    internal class CreateSeriesValidator : AbstractValidator<CreateSeriesCommand>
    {
        public CreateSeriesValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull()
                .WithMessage("Dto cannot be null");

            this.When(
                p => p.WriteModel != null,
                () => this.RuleFor(p => p.WriteModel)
                    .SetValidator(new SeriesValidator()));
        }
    }
}