using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Queries.Shelves;
using FluentValidation;

namespace BookLovers.Bookcases.Infrastructure.Validators.Queries
{
    internal class BookOnShelvesQueryValidator : AbstractValidator<BookOnShelvesQuery>
    {
        public BookOnShelvesQueryValidator()
        {
            RuleFor(p => p.Count).GreaterThanOrEqualTo(0)
                .WithMessage("Items per page must be greater or equal 0");

            RuleFor(p => p.Page)
                .GreaterThanOrEqualTo(PaginatedResult.DefaultPage)
                .WithMessage($"Page must be greater or equal {PaginatedResult.DefaultPage}");
        }
    }
}