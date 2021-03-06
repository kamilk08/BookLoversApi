using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.PublisherCycles;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Queries.Cycles
{
    internal class FindCycleQueryValidator : AbstractValidator<FindCycleQuery>
    {
        public FindCycleQueryValidator()
        {
            this.RuleFor(p => p.Count).GreaterThanOrEqualTo(0)
                .WithMessage("Items per page must be greater or equal 0");

            this.RuleFor(p => p.Page)
                .GreaterThanOrEqualTo(PaginatedResult.DefaultPage)
                .WithMessage($"Page must be greater or equal {PaginatedResult.DefaultPage}");
        }
    }
}