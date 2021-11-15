using BookLovers.Ratings.Application.Commands;
using FluentValidation;

namespace BookLovers.Ratings.Infrastructure.Validators.Commands
{
    internal class ChangeRatingValidator : AbstractValidator<ChangeRatingCommand>
    {
        public ChangeRatingValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null.");

            this.When(
                p => p.WriteModel != null,
                () => this.RuleFor(p => p.WriteModel.BookGuid)
                    .NotNull().NotEmpty().WithMessage("Invalid book guid."));
        }
    }
}