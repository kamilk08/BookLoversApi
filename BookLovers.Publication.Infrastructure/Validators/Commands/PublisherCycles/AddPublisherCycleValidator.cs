using BookLovers.Publication.Application.Commands.PublisherCycles;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.PublisherCycles
{
    internal class AddPublisherCycleValidator : AbstractValidator<AddPublisherCycleCommand>
    {
        public AddPublisherCycleValidator()
        {
            this.RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null");

            this.When(p => p.WriteModel != null, () =>
            {
                this.RuleFor(p => p.WriteModel.CycleGuid).NotNull()
                    .WithMessage("CycleGuid cannot be null").NotEmpty()
                    .WithMessage("CycleGuid cannot be empty");

                this.RuleFor(p => p.WriteModel.Cycle).NotNull()
                    .WithMessage("Cycle name cannot be null").NotEmpty()
                    .WithMessage("Cycle name cannot be empty").MinimumLength(3)
                    .WithMessage("Cycle name should have at least 3 characters")
                    .MaximumLength(byte.MaxValue)
                    .WithMessage("Cycle name cannot have more then 255 characters");

                this.RuleFor(p => p.WriteModel.PublisherGuid).NotNull()
                    .WithMessage("PublisherGuid cannot be null").NotEmpty()
                    .WithMessage("PublisherGuid cannot be empty");
            });
        }
    }
}