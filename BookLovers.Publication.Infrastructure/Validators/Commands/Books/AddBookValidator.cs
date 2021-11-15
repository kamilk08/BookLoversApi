using BookLovers.Publication.Application.Commands.Books;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Books
{
    internal class AddBookValidator : AbstractValidator<AddBookCommand>
    {
        public AddBookValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull()
                .WithMessage("Dto cannot be null");

            this.When(
                p => p.WriteModel != null,
                () => this.RuleFor(p => p.WriteModel.BookWriteModel).NotNull()
                    .WithMessage("Book dto object cannot be null")
                    .SetValidator(new BookValidator()));
        }
    }
}