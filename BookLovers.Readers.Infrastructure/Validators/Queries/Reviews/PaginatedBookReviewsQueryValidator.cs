using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Queries.Reviews
{
    internal class PaginatedBookReviewsQueryValidator : AbstractValidator<PaginatedBookReviewsQuery>
    {
        public PaginatedBookReviewsQueryValidator()
        {
            this.RuleFor(p => p.Count).GreaterThanOrEqualTo(0)
                .WithMessage("Items per page must be greater or equal 0");

            this.RuleFor(p => p.Page)
                .GreaterThanOrEqualTo(PaginatedResult.DefaultPage)
                .WithMessage($"Page must be greater or equal {PaginatedResult.DefaultPage}");
        }
    }
}