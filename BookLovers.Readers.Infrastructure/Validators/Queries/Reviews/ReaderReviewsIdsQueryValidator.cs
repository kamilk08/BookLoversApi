using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Queries.Reviews
{
    internal class ReaderReviewsIdsQueryValidator : AbstractValidator<ReaderReviewsIdsQuery>
    {
        public ReaderReviewsIdsQueryValidator()
        {
            this.RuleFor(p => p.Count)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Items per page must be greater or equal 0");

            this.RuleFor(p => p.Page)
                .GreaterThanOrEqualTo(PaginatedResult.DefaultPage)
                .WithMessage(string.Format("Page must be greater or equal {0}", PaginatedResult.DefaultPage));
        }
    }
}