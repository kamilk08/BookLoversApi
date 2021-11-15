using BookLovers.Ratings.Infrastructure.Queries.Series;
using FluentValidation;

namespace BookLovers.Ratings.Infrastructure.Validators.Queries
{
    internal class MultipleSeriesStatisticsQueryValidator :
        AbstractValidator<MultipleSeriesStatisticsQuery>
    {
        public MultipleSeriesStatisticsQueryValidator()
        {
            RuleFor(p => p.SeriesIds).NotNull()
                .WithMessage("Invalids series ids.");
        }
    }
}