using BookLovers.Readers.Application.WriteModels.Profiles;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.SocialProfile
{
    internal class FavouritesValidator : AbstractValidator<FavouritesWriteModel>
    {
        public FavouritesValidator()
        {
            this.RuleFor(p => p.Authors).NotNull().WithMessage("Authors cannot be null");

            this.RuleFor(p => p.Books).NotNull().WithMessage("Books cannot be null");

            this.RuleFor(p => p.SubCategories)
                .NotNull().WithMessage("Subcategories cannot be null");
        }
    }
}