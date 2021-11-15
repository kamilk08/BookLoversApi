using BookLovers.Publication.Infrastructure.Queries.Series;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Queries.Series
{
    internal class MultipleSeriesQueryValidator : AbstractValidator<MultipleSeriesQuery>
    {
        public MultipleSeriesQueryValidator()
        {
            this.RuleFor(p => p.Ids)
                .NotNull()
                .WithMessage("Invalid series ids");
        }
    }
}