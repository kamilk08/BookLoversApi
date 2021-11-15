using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.Authors;
using BookLovers.Publication.Infrastructure.Services.AuthorBooksSortingServices;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Queries.Authors
{
    internal class AuthorsCollectionQueryValidator : AbstractValidator<AuthorsCollectionQuery>
    {
        public AuthorsCollectionQueryValidator()
        {
            this.RuleFor(p => p.Count).GreaterThanOrEqualTo(0)
                .WithMessage("Items per page must be greater or equal 0");

            this.RuleFor(p => p.Page).GreaterThanOrEqualTo(PaginatedResult.DefaultPage)
                .WithMessage($"Page must be greater or equal {PaginatedResult.DefaultPage}");

            this.RuleFor(p => p.Descending)
                .NotNull().WithMessage("Sort order must not be null.");

            this.RuleFor(p => p.SortType)
                .GreaterThanOrEqualTo(AuthorCollectionSorType.ByTitle.Value)
                .LessThanOrEqualTo(AuthorCollectionSorType.ByAverage.Value)
                .WithMessage("Invalid sort type.");
        }
    }
}