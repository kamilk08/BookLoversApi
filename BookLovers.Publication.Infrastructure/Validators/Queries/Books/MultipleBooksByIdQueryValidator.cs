using BookLovers.Publication.Infrastructure.Queries.Books;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Queries.Books
{
    internal class MultipleBooksByIdQueryValidator : AbstractValidator<MultipleBooksByIdQuery>
    {
        public MultipleBooksByIdQueryValidator()
        {
            this.RuleFor(p => p.BookIds).NotNull().WithMessage("Invalid book ids.");
        }
    }
}