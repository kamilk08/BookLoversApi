using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Validation;
using BookLovers.Readers.Application.WriteModels.Profiles;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.SocialProfile
{
    internal class SocialMediaValidator : AbstractValidator<AboutWriteModel>
    {
        public SocialMediaValidator()
        {
            this.RuleFor(p => p.Content).MinimumLength(3)
                .WithMessage("User about information should have atleast 3 characters")
                .MaximumLength(500)
                .WithMessage("User about information cannot contain more then 500 characters")
                .When(p => !p.Content.IsEmpty());

            this.RuleFor(p => p.WebSite).Must(UrlValidator.IsValidUrl)
                .When(p => !p.WebSite.IsEmpty()).WithMessage("Invalid website url");
        }
    }
}