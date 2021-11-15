using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Queries.Librarians;
using FluentValidation;

namespace BookLovers.Librarians.Infrastructure.Validators.Queries
{
    internal class ManageableTicketsQueryValidator : AbstractValidator<ManageableTicketsQuery>
    {
        public ManageableTicketsQueryValidator()
        {
            this.RuleFor(p => p.Count)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Items per page must be greater or equal 0");

            this.RuleFor(p => p.Page)
                .GreaterThanOrEqualTo(PaginatedResult.DefaultPage)
                .WithMessage($"Page must be greater or equal {PaginatedResult.DefaultPage}");

            this.RuleFor(p => p.Solved)
                .NotNull().WithMessage("Invalid 'Solved' value");
        }
    }
}