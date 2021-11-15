using BookLovers.Publication.Application.Commands.Publishers;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Publisher
{
    internal class ArchivePublisherValidator : AbstractValidator<ArchivePublisherCommand>
    {
        public ArchivePublisherValidator()
        {
            this.RuleFor(p => p.PublisherGuid)
                .NotEmpty()
                .WithMessage("Publisher guid cannot be empty");
        }
    }
}