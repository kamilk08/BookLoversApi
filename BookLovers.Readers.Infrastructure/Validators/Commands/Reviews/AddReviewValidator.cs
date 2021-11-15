using BookLovers.Readers.Application.Commands.Reviews;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Reviews
{
    internal class AddReviewValidator : AbstractValidator<AddReviewCommand>
    {
        public AddReviewValidator()
        {
            this.RuleFor(p => p.WriteModel)
                .NotNull()
                .WithMessage("Dto cannot be null");

            this.When(
                p => p.WriteModel != null,
                () =>
                    this.RuleFor(p => p.WriteModel)
                        .SetValidator(new ReviewValidator()));
        }
    }
}