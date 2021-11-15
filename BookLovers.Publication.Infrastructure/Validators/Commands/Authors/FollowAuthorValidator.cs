using BookLovers.Publication.Application.Commands.Authors;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Authors
{
    internal class FollowAuthorValidator : AbstractValidator<FollowAuthorCommand>
    {
        public FollowAuthorValidator()
        {
            this.RuleFor(p => p.AuthorGuid).NotEmpty()
                .WithMessage("Author guid cannot be empty");
        }
    }
}