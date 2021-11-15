using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Queries.Ratings;
using FluentValidation;

namespace BookLovers.Ratings.Infrastructure.Validators.Queries
{
    internal class ReaderRatingsByIdQueryValidator : AbstractValidator<ReaderRatingsByIdQuery>
    {
        public ReaderRatingsByIdQueryValidator()
        {
            this.RuleFor(p => p.Count)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Items per page must be greater or equal 0");

            this.RuleFor(p => p.Page)
                .GreaterThanOrEqualTo(PaginatedResult.DefaultPage)
                .WithMessage($"Page must be greater or equal {PaginatedResult.DefaultPage}");
        }
    }
}