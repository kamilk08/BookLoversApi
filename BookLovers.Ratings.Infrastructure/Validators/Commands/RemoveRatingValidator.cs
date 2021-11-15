using BookLovers.Ratings.Application.Commands;
using FluentValidation;

namespace BookLovers.Ratings.Infrastructure.Validators.Commands
{
    internal class RemoveRatingValidator : AbstractValidator<RemoveRatingCommand>
    {
        public RemoveRatingValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null");

            this.When(
                p => p.WriteModel != null,
                () => this.RuleFor(p => p.WriteModel.BookGuid)
                    .NotNull().NotEmpty().WithMessage("Invalid book guid."));
        }
    }
}