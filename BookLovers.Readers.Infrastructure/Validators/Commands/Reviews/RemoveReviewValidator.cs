using BookLovers.Readers.Application.Commands.Reviews;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Reviews
{
    internal class RemoveReviewValidator : AbstractValidator<RemoveReviewCommand>
    {
        public RemoveReviewValidator()
        {
            this.RuleFor(p => p.ReviewGuid)
                .NotEmpty().WithMessage("Review guid cannot be empty")
                .NotNull().WithMessage("Review guid cannot be null");
        }
    }
}