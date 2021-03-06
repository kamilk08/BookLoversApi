using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.Publishers;
using BookLovers.Publication.Infrastructure.Services.PublisherBooksSortingServices;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Queries.Publishers
{
    internal class PaginatedPublishersCollectionQueryValidator :
        AbstractValidator<PaginatedPublishersCollectionQuery>
    {
        public PaginatedPublishersCollectionQueryValidator()
        {
            this.RuleFor(p => p.Count)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Items per page must be greater or equal 0");

            this.RuleFor(p => p.Page)
                .GreaterThanOrEqualTo(PaginatedResult.DefaultPage).WithMessage(
                    $"Page must be greater or equal {PaginatedResult.DefaultPage}");

            this.RuleFor(p => p.Descending)
                .NotNull().WithMessage("Sort order must not be null.");

            this.RuleFor(p => p.SortType)
                .GreaterThanOrEqualTo(PublisherCollectionSortingType.ByTitle.Value)
                .LessThanOrEqualTo(PublisherCollectionSortingType.ByPublicationDate.Value)
                .WithMessage("Invalid sort type.");
        }
    }
}