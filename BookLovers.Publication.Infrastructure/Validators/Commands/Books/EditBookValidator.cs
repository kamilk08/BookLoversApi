using BookLovers.Publication.Application.Commands.Books;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Books
{
    internal class EditBookValidator : AbstractValidator<EditBookCommand>
    {
        public EditBookValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull()
                .WithMessage("Dto cannot be null");

            this.When(
                p => p.WriteModel != null,
                () => this
                    .RuleFor(p => p.WriteModel.BookWriteModel)
                    .SetValidator(new BookValidator()));
        }
    }
}