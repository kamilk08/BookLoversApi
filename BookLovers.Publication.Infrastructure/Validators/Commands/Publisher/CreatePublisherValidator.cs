using BookLovers.Publication.Application.Commands.Publishers;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Publisher
{
    internal class CreatePublisherValidator : AbstractValidator<CreatePublisherCommand>
    {
        public CreatePublisherValidator()
        {
            this.RuleFor(p => p.WriteModel)
                .NotNull().WithMessage("Dto cannot be null");

            this.When(
                p => p.WriteModel != null,
                () => this.RuleFor(p => p.WriteModel)
                    .SetValidator(new PublisherValidator()));
        }
    }
}