using BookLovers.Publication.Application.Commands.PublisherCycles;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.PublisherCycles
{
    internal class RemovePublisherCycleBookValidator :
        AbstractValidator<RemovePublisherCycleBookCommand>
    {
        public RemovePublisherCycleBookValidator()
        {
            this.RuleFor(p => p.BookGuid).NotNull().NotEmpty()
                .WithMessage("Invalid book guid");

            this.RuleFor(p => p.CycleGuid).NotNull().NotEmpty()
                .WithMessage("Invalid cycle guid");
        }
    }
}