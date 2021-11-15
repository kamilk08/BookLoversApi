using BookLovers.Readers.Application.Commands.Readers;
using FluentValidation;

namespace BookLovers.Readers.Infrastructure.Validators.Commands.Readers
{
    internal class UnFollowReaderValidator : AbstractValidator<UnFollowReaderCommand>
    {
        public UnFollowReaderValidator()
        {
            this.RuleFor(p => p.Dto != default)
                .NotNull().WithMessage("Data transferable object cannot be null");

            this.When(
                p => p.Dto != null,
                () => this.RuleFor(p => p.Dto).SetValidator(new FollowValidator()));
        }
    }
}