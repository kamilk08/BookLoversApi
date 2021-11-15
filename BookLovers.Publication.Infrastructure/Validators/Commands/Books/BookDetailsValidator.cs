using BookLovers.Publication.Application.WriteModels.Books;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Books
{
    internal class BookDetailsValidator : AbstractValidator<BookDetailsWriteModel>
    {
        public BookDetailsValidator()
        {
            RuleFor(p => p.Pages.GetValueOrDefault())
                .GreaterThan(0).WithMessage("Book's pages cannot be less then zero")
                .When(p => p.Pages != default(int));
        }
    }
}