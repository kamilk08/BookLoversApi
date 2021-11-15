using BookLovers.Readers.Application.Commands.Profile;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.SocialProfile
{
    internal class EditSocialProfileValidator : AbstractValidator<EditProfileCommand>
    {
        public EditSocialProfileValidator()
        {
            this.RuleFor(p => p.WriteModel)
                .NotNull().WithMessage("Data transferable object cannot be null");

            this.When(
                p => p.WriteModel != null,
                () => this.RuleFor(p => p.WriteModel)
                    .NotNull()
                    .WithMessage("Social profile dto cannot be null")
                    .SetValidator(new SocialProfileValidator()));
        }
    }
}