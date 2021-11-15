using BookLovers.Readers.Application.Commands.Readers;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Readers
{
    internal class FollowReaderValidator : AbstractValidator<FollowReaderCommand>
    {
        public FollowReaderValidator()
        {
            this.RuleFor(p => p.Dto)
                .NotNull()
                .WithMessage("Dto cannot be null");

            this.When(p => p.Dto != null, () => this.RuleFor(p => p.Dto)
                .SetValidator(new FollowValidator()));
        }
    }
}