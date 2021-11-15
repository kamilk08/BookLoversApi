using BookLovers.Readers.Application.WriteModels.Reviews;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Reviews
{
    internal class ReviewValidator : AbstractValidator<ReviewWriteModel>
    {
        public ReviewValidator()
        {
            this.RuleFor(p => p.BookGuid).NotEmpty().WithMessage("BookGuid cannot be empty");

            this.RuleFor(p => p.ReviewGuid).NotEmpty().WithMessage("ReviewGuid cannot be empty");

            this.RuleFor(p => p.ReviewDetails).NotNull().WithMessage("Review details dto cannot be null")
                .SetValidator(new ReviewDetailsValidator());
        }
    }
}