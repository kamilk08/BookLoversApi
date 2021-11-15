using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;
using BookLovers.Bookcases.Infrastructure.Services;
using FluentValidation;

namespace BookLovers.Bookcases.Infrastructure.Validators.Queries
{
    internal class PaginatedBookcaseCollectionQueryValidator :
        AbstractValidator<PaginatedBookcaseCollectionQuery>
    {
        public PaginatedBookcaseCollectionQueryValidator()
        {
            RuleFor(p => p.Count).GreaterThanOrEqualTo(0).WithMessage("Items per page must be greater or equal 0");

            RuleFor(p => p.Page).GreaterThanOrEqualTo(PaginatedResult.DefaultPage)
                .WithMessage($"Page must be greater or equal {PaginatedResult.DefaultPage}");

            RuleFor(p => p.Descending).NotNull().WithMessage("Sort order must not be null.");

            RuleFor(p => p.BookIds).NotNull().WithMessage("Invalid book ids.");

            RuleFor(p => p.ShelvesIds).NotNull().WithMessage("Invalid shelves ids.");

            RuleFor(p => p.SortType).GreaterThanOrEqualTo(BookcaseCollectionSortType.ByDate.Value)
                .LessThanOrEqualTo(BookcaseCollectionSortType.ByBookAverage.Value).WithMessage("Invalid sort type.");
        }
    }
}