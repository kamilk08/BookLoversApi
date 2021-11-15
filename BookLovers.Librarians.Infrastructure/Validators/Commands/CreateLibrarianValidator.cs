using BookLovers.Librarians.Application.Commands;
using FluentValidation;

namespace BookLovers.Librarians.Infrastructure.Validators.Commands
{
    internal class CreateLibrarianValidator : AbstractValidator<CreateLibrarianCommand>
    {
        public CreateLibrarianValidator()
        {
            this.RuleFor(p => p.WriteModel)
                .NotNull()
                .WithMessage("Dto cannot be null");

            this.When(p => p.WriteModel != null, () =>
            {
                this.RuleFor(p => p.WriteModel.ReaderGuid)
                    .NotEmpty().WithMessage("Reader Guid cannot be empty");

                this.RuleFor(p => p.WriteModel.LibrarianGuid)
                    .NotEmpty().WithMessage("Librarian Guid cannot be empty");
            });
        }
    }
}