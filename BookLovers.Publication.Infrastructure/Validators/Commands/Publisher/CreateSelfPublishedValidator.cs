using BookLovers.Publication.Application.Commands.Publishers;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Publisher
{
    internal class CreateSelfPublishedValidator : AbstractValidator<CreateSelfPublisherCommand>
    {
        public CreateSelfPublishedValidator()
        {
            this.RuleFor(p => p.PublisherGuid)
                .NotEmpty();
        }
    }
}