using System;
using BookLovers.Base.Infrastructure.Validation;
using BookLovers.Readers.Application.WriteModels.Reviews;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Reviews
{
    internal class ReviewDetailsValidator : AbstractValidator<ReviewDetailsWriteModel>
    {
        public ReviewDetailsValidator()
        {
            this.RuleFor(p => p.Content).NotEmpty()
                .WithMessage("Review cannot be empty")
                .NotNull().WithMessage("Review cannot be null")
                .MinimumLength(3).WithMessage("Review should contain at least 3 characters");

            this.RuleFor(p => p.ReviewDate)
                .NotNull().WithMessage("Review date cannot be null")
                .NotEqual(default(DateTime))
                .WithMessage("Invalid review date").Must(DateValidator.IsValidDate)
                .WithMessage("Review date is invalid");
        }
    }
}