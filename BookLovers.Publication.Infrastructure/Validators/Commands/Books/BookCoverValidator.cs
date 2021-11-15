using BookLovers.Publication.Application.WriteModels.Books;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Books
{
    internal class BookCoverValidator : AbstractValidator<BookCoverWriteModel>
    {
        public BookCoverValidator()
        {
            this.RuleFor(p => p.CoverSource).NotEmpty()
                .When(p => p.IsCoverAdded)
                .WithMessage("Book cover source cannot be empty when cover is set.");
        }
    }
}