using BookLovers.Readers.Application.Commands.Reviews;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Reviews
{
    internal class ReportReviewValidator : AbstractValidator<ReportReviewCommand>
    {
        public ReportReviewValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null.");

            this.When(
                p => p.WriteModel != null,
                () => this.RuleFor(p => p.WriteModel.ReviewGuid)
                    .NotEmpty().WithMessage("Guid cannot be null"));
        }
    }
}