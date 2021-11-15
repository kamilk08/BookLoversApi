using BookLovers.Publication.Application.WriteModels.Publisher;
using FluentValidation;

namespace BookLovers.Publication.Infrastructure.Validators.Commands.Publisher
{
    internal class PublisherValidator : AbstractValidator<AddPublisherWriteModel>
    {
        public PublisherValidator()
        {
            this.RuleFor(p => p.PublisherGuid)
                .NotEmpty()
                .WithMessage("Publisher guid cannot be empty");

            this.RuleFor(p => p.Name).NotNull()
                .WithMessage("Publisher name cannot be null")
                .NotEmpty().WithMessage("Publisher name cannot be empty")
                .MinimumLength(3)
                .WithMessage("Publisher name should have at least 3 characters")
                .MaximumLength(byte.MaxValue)
                .WithMessage("Publisher name cannot have more then 255 characters");
        }
    }
}