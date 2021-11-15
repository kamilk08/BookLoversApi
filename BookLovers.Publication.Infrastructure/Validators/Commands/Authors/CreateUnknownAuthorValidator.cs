using BookLovers.Publication.Application.Commands.Authors;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Authors
{
    internal class CreateUnknownAuthorValidator : AbstractValidator<CreateUnknownAuthorCommand>
    {
        public CreateUnknownAuthorValidator()
        {
            this.RuleFor(p => p.AuthorGuid).NotEmpty();
        }
    }
}