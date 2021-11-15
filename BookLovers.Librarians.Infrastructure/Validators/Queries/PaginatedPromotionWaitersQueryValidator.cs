using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Queries.PromotionWaiters;
using FluentValidation;

namespace BookLovers.Librarians.Infrastructure.Validators.Queries
{
    internal class PaginatedPromotionWaitersQueryValidator :
        AbstractValidator<PaginatedPromotionWaitersQuery>
    {
        public PaginatedPromotionWaitersQueryValidator()
        {
            this.RuleFor(p => p.Count)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Items per page must be greater or equal 0");

            this.RuleFor(p => p.Page)
                .GreaterThanOrEqualTo(PaginatedResult.DefaultPage)
                .WithMessage($"Page must be greater or equal {PaginatedResult.DefaultPage}");

            this.RuleFor(p => p.Ids)
                .NotNull().WithMessage("Invalid ids query property.");
        }
    }
}