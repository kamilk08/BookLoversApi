using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Queries.Readers.Follows;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Queries.Readers
{
    internal class ReaderPaginatedFollowingsQueryValidator :
        AbstractValidator<ReaderPaginatedFollowingsQuery>
    {
        public ReaderPaginatedFollowingsQueryValidator()
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