using BookLovers.Publication.Application.Commands.PublisherCycles;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.PublisherCycles
{
    internal class ArchivePublisherCycleValidator : AbstractValidator<ArchivePublisherCycleCommand>
    {
        public ArchivePublisherCycleValidator()
        {
            this.RuleFor(p => p.PublisherCycleGuid).NotEmpty()
                .WithMessage("Publisher guid cannot be empty");
        }
    }
}