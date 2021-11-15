using BookLovers.Publication.Infrastructure.Queries.Authors;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Queries.Authors
{
    internal class MultipleAuthorsByGuidQueryValidator : AbstractValidator<MultipleAuthorsByGuidQuery>
    {
        public MultipleAuthorsByGuidQueryValidator()
        {
            this.RuleFor(p => p.Guides).NotNull().WithMessage("Invalid author guides.");
        }
    }
}