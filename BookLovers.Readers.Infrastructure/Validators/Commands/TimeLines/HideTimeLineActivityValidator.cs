using BookLovers.Readers.Application.Commands.Timelines;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.TimeLines
{
    internal class HideTimeLineActivityValidator : AbstractValidator<HideActivityCommand>
    {
        public HideTimeLineActivityValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null");

            this.When(p => p.WriteModel != null, () =>
            {
                this.RuleFor(p => p.WriteModel.OccuredAt)
                    .NotNull().WithMessage("Activity date cannot be null");

                this.RuleFor(p => p.WriteModel.TimeLineObjectGuid)
                    .NotNull().NotEmpty()
                    .WithMessage("Activity object guid cannot be null");
            });
        }
    }
}