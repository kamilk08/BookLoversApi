using BookLovers.Readers.Application.Commands.Reviews;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Reviews
{
    internal class UnlikeReviewValidator : AbstractValidator<UnlikeReviewCommand>
    {
        public UnlikeReviewValidator()
        {
            this.RuleFor(p => p.ReviewGuid)
                .NotEmpty().WithMessage("Review guid cannot be empty");
        }
    }
}