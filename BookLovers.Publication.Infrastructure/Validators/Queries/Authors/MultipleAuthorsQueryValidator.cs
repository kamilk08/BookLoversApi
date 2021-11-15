using BookLovers.Publication.Infrastructure.Queries.Authors;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Queries.Authors
{
    internal class MultipleAuthorsQueryValidator : AbstractValidator<MultipleAuthorsQuery>
    {
        public MultipleAuthorsQueryValidator()
        {
            this.RuleFor(p => p.Ids).NotNull()
                .WithMessage("Invalid author ids.");
        }
    }
}