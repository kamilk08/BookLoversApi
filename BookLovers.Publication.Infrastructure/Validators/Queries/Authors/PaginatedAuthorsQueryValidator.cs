using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.Authors;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Queries.Authors
{
    internal class PaginatedAuthorsQueryValidator : AbstractValidator<PaginatedAuthorsQuery>
    {
        public PaginatedAuthorsQueryValidator()
        {
            this.RuleFor(p => p.Value).NotEmpty()
                .NotNull().WithMessage("Invalid author query");

            this.RuleFor(p => p.Count)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Items per page must be greater or equal 0");

            this.RuleFor(p => p.Page)
                .GreaterThanOrEqualTo(PaginatedResult.DefaultPage)
                .WithMessage($"Page must be greater or equal {PaginatedResult.DefaultPage}");
        }
    }
}