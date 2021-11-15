using BookLovers.Readers.Application.Commands.Reviews;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Reviews
{
    internal class AddSpoilerTagValidator : AbstractValidator<AddSpoilerTagCommand>
    {
        public AddSpoilerTagValidator()
        {
            this.RuleFor(p => p.ReviewGuid).NotEmpty()
                .WithMessage("Invalid review guid.");
        }
    }
}