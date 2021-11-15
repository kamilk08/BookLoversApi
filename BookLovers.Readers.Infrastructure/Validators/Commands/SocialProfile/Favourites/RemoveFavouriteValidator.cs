using BookLovers.Readers.Application.Commands.Profile;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.SocialProfile.Favourites
{
    internal class RemoveFavouriteValidator : AbstractValidator<RemoveFavouriteCommand>
    {
        public RemoveFavouriteValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null");

            this.When(p => p.WriteModel != null, () =>
            {
                this.RuleFor(p => p.WriteModel.FavouriteGuid)
                    .NotEmpty().WithMessage("Favourite guid cannot be empty");

                this.RuleFor(p => p.WriteModel.ProfileGuid)
                    .NotEmpty().WithMessage("Favourite profile guid cannot be empty");
            });
        }
    }
}