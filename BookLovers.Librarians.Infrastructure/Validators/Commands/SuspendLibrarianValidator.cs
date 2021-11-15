using BookLovers.Librarians.Application.Commands;
using FluentValidation;

namespace BookLovers.Librarians.Infrastructure.Validators.Commands
{
    internal class SuspendLibrarianValidator : AbstractValidator<SuspendLibrarianCommand>
    {
        public SuspendLibrarianValidator()
        {
            this.RuleFor(p => p.UserGuid)
                .NotEmpty().WithMessage("User guid cannot be empty");
        }
    }
}