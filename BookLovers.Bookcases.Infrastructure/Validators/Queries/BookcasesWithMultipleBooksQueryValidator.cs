using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;
using FluentValidation;

namespace BookLovers.Bookcases.Infrastructure.Validators.Queries
{
    internal class BookcasesWithMultipleBooksQueryValidator :
        AbstractValidator<BookcasesWithMultipleBooksQuery>
    {
        public BookcasesWithMultipleBooksQueryValidator()
        {
            RuleFor(p => p.BookIds).NotNull().WithMessage("Invalid book ids.");
        }
    }
}