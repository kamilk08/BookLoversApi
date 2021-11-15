using BookLovers.Publication.Application.Commands.PublisherCycles;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.PublisherCycles
{
    internal class AddPublisherCycleBookValidator : AbstractValidator<AddPublisherCycleBookCommand>
    {
        public AddPublisherCycleBookValidator()
        {
            this.RuleFor(p => p.BookGuid).NotNull()
                .NotEmpty()
                .WithMessage("Invalid book guid");

            this.RuleFor(p => p.CycleGuid).NotNull()
                .NotEmpty()
                .WithMessage("Invalid cycle guid");
        }
    }
}