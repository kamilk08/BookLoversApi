using BookLovers.Publication.Application.Commands.Authors;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Authors
{
    internal class ArchiveAuthorValidator : AbstractValidator<ArchiveAuthorCommand>
    {
        public ArchiveAuthorValidator()
        {
            this.RuleFor(p => p.AuthorGuid).NotEmpty()
                .NotNull().WithMessage("Invalid author guid.");
        }
    }
}