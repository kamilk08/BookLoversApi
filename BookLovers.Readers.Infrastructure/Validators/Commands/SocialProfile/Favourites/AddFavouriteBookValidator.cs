using System;
using BookLovers.Base.Infrastructure.Validation;
using BookLovers.Readers.Application.Commands.Profile;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.SocialProfile.Favourites
{
    internal class AddFavouriteBookValidator : AbstractValidator<AddFavouriteBookCommand>
    {
        public AddFavouriteBookValidator()
        {
            this.RuleFor(p => p.WriteModel != default)
                .NotNull().WithMessage("Dto cannot be null");

            this.When(p => p.WriteModel != null, () =>
            {
                this.RuleFor(p => p.WriteModel.BookGuid).NotNull()
                    .NotEmpty().WithMessage("Book guid cannot be empty or null");

                this.RuleFor(p => p.WriteModel.ProfileGuid).NotNull()
                    .NotEmpty().WithMessage("Profile guid cannot be empty or null");

                this.RuleFor(p => p.WriteModel.AddedAt)
                    .Must(p => p != default(DateTime))
                    .Must(DateValidator.IsValidDate)
                    .WithMessage("Invalid date");
            });
        }
    }
}