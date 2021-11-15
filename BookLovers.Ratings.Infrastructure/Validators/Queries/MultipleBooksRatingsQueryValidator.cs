using BookLovers.Ratings.Infrastructure.Queries.Books;
using FluentValidation;

namespace BookLovers.Ratings.Infrastructure.Validators.Queries
{
    internal class MultipleBooksRatingsQueryValidator : AbstractValidator<MultipleBooksRatingsQuery>
    {
        public MultipleBooksRatingsQueryValidator()
        {
            this.RuleFor(p => p.BookIds)
                .NotNull()
                .WithMessage("Invalid book ids.");
        }
    }
}