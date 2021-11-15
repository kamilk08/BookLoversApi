using BookLovers.Readers.Infrastructure.Queries.Readers.Profiles;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Queries.Readers
{
    internal class FavouriteAuthorsQueryValidator : AbstractValidator<FavouriteAuthorsQuery>
    {
        public FavouriteAuthorsQueryValidator()
        {
            RuleFor(p => p.ReaderId).NotEmpty()
                .NotNull()
                .WithMessage("Invalid id");
        }
    }
}