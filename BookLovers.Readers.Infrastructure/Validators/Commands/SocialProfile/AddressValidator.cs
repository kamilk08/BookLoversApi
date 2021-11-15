using BookLovers.Readers.Application.WriteModels.Profiles;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.SocialProfile
{
    internal class AddressValidator : AbstractValidator<AddressWriteModel>
    {
        public AddressValidator()
        {
            this.RuleFor(p => p.City).MaximumLength(255)
                .WithMessage("City cannot be longer then 255 characters");

            this.RuleFor(p => p.Country).MaximumLength(255)
                .WithMessage("Country cannot be longer then 255 characters");
        }
    }
}