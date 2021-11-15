using BookLovers.Readers.Application.Commands.Reviews;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Reviews
{
    internal class EditReviewValidator : AbstractValidator<EditReviewCommand>
    {
        public EditReviewValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null");

            this.When(
                p => p.WriteModel != null,
                () => this.RuleFor(p => p.WriteModel)
                    .SetValidator(new ReviewValidator()));
        }
    }
}