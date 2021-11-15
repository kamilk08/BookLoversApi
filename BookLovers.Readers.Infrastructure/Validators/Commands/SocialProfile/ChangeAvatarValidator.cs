using BookLovers.Readers.Application.Commands.Profile;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.SocialProfile
{
    internal class ChangeAvatarValidator : AbstractValidator<ChangeAvatarCommand>
    {
        public ChangeAvatarValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull()
                .WithMessage("Invalid avatar");
        }
    }
}