using BookLovers.Publication.Infrastructure.Queries.Books;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Queries.Books
{
    internal class MultipleBooksByGuidQueryValidator : AbstractValidator<MultipleBooksByGuidQuery>
    {
        public MultipleBooksByGuidQueryValidator()
        {
            this.RuleFor(p => p.Guides).NotNull().WithMessage("Invalid book guides.");
        }
    }
}