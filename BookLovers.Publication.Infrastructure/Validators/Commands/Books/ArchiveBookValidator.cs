using BookLovers.Publication.Application.Commands.Books;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Books
{
    internal class ArchiveBookValidator : AbstractValidator<ArchiveBookCommand>
    {
        public ArchiveBookValidator()
        {
            this.RuleFor(p => p.BookGuid).NotEmpty()
                .WithMessage("Book guid cannot be empty.");
        }
    }
}