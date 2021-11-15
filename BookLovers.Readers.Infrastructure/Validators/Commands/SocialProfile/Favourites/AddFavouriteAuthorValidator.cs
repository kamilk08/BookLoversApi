using System;
using BookLovers.Base.Infrastructure.Validation;
using BookLovers.Readers.Application.Commands.Profile;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.SocialProfile.Favourites
{
    internal class AddFavouriteAuthorValidator : AbstractValidator<AddFavouriteAuthorCommand>
    {
        public AddFavouriteAuthorValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull().WithMessage("Invalid dto");
            this.When(p => p.WriteModel != null, () =>
            {
                this.RuleFor(p => p.WriteModel.AddedAt)
                    .Must(p => p != default(DateTime))
                    .Must(DateValidator.IsValidDate)
                    .WithMessage("Invalid date");

                this.RuleFor(p => p.WriteModel.AuthorGuid).NotEmpty()
                    .WithMessage("Author guid cannot be empty");

                this.RuleFor(p => p.WriteModel.ProfileGuid).NotEmpty()
                    .WithMessage("Profile guid cannot be empty");
            });
        }
    }
}