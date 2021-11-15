using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.Publishers;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Queries.Publishers
{
    internal class FindPublisherQueryValidator : AbstractValidator<FindPublisherQuery>
    {
        public FindPublisherQueryValidator()
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