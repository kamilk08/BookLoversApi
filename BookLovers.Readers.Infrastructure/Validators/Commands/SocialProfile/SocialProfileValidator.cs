using BookLovers.Readers.Application.WriteModels.Profiles;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.SocialProfile
{
    internal class SocialProfileValidator : AbstractValidator<ProfileWriteModel>
    {
        public SocialProfileValidator()
        {
            this.RuleFor(p => p.ProfileGuid)
                .NotNull().WithMessage("SocialProfileGuid cannot be null")
                .NotEmpty().WithMessage("SocialProfileGuid cannot be empty");

            this.RuleFor(p => p.Address)
                .NotNull()
                .WithMessage("Address dto cannot be null");

            this.When(
                p => p.Address != null,
                () => this.RuleFor(p => p.Address)
                    .SetValidator(new AddressValidator()));

            this.RuleFor(p => p.Identity).NotNull()
                .WithMessage("Identity dto cannot be null");

            this.When(
                p => p.Identity != null,
                () => this.RuleFor(p => p.Identity).SetValidator(new IdentityValidator()));

            this.RuleFor(p => p.About).NotNull()
                .WithMessage("Social media dto cannot be null");

            this.When(
                p => p.About != null,
                () => this.RuleFor(p => p.About).SetValidator(new SocialMediaValidator()));
        }
    }
}