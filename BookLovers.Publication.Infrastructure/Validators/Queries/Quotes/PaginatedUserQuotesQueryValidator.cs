using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.Quotes;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Queries.Quotes
{
    internal class PaginatedUserQuotesQueryValidator : AbstractValidator<PaginatedUserQuotesQuery>
    {
        public PaginatedUserQuotesQueryValidator()
        {
            this.RuleFor(p => p.Count)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Items per page must be greater or equal 0");

            this.RuleFor(p => p.Page)
                .GreaterThanOrEqualTo(PaginatedResult.DefaultPage)
                .WithMessage($"Page must be greater or equal {PaginatedResult.DefaultPage}");

            this.RuleFor(p => p.Descending)
                .NotNull().WithMessage("Sort order must not be null.");

            this.RuleFor(p => p.Order)
                .GreaterThanOrEqualTo(QuotesOrder.ById.Value)
                .LessThanOrEqualTo(QuotesOrder.ByLikes.Value)
                .WithMessage("Invalid sort type.");
        }
    }
}